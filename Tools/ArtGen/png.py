import struct, zlib


def read(path):
    data = open(path, 'rb').read()
    pos, idat, w, h, ct = 8, b'', 0, 0, 0
    plte = []
    while pos < len(data):
        n, t = struct.unpack('>I4s', data[pos:pos + 8])
        body = data[pos + 8:pos + 8 + n]
        if t == b'IHDR':
            w, h, bd, ct = struct.unpack('>IIBB', body[:10])
        elif t == b'PLTE':
            plte = [tuple(body[i:i + 3]) + (255,) for i in range(0, n, 3)]
        elif t == b'IDAT':
            idat += body
        pos += 12 + n
    bpp = {6: 4, 2: 3, 3: 1, 0: 1, 4: 2}[ct]
    raw = zlib.decompress(idat)
    stride = w * bpp
    prev = bytearray(stride)
    rows = []
    i = 0
    for _ in range(h):
        f = raw[i]; line = bytearray(raw[i + 1:i + 1 + stride]); i += 1 + stride
        for x in range(stride):
            a = line[x - bpp] if x >= bpp else 0
            b = prev[x]
            c = prev[x - bpp] if x >= bpp else 0
            if f == 1: line[x] = (line[x] + a) & 255
            elif f == 2: line[x] = (line[x] + b) & 255
            elif f == 3: line[x] = (line[x] + (a + b) // 2) & 255
            elif f == 4:
                p = a + b - c; pa, pb, pc = abs(p - a), abs(p - b), abs(p - c)
                pr = a if pa <= pb and pa <= pc else (b if pb <= pc else c)
                line[x] = (line[x] + pr) & 255
        rows.append(line); prev = line
    px = []
    for line in rows:
        r = []
        for x in range(w):
            v = tuple(line[x * bpp:x * bpp + bpp])
            if ct == 6: r.append(v)
            elif ct == 2: r.append(v + (255,))
            elif ct == 3: r.append(plte[v[0]])
            elif ct == 0: r.append((v[0],) * 3 + (255,))
            else: r.append((v[0],) * 3 + (v[1],))
        px.append(r)
    return w, h, px


def write(path, w, h, px):
    raw = b''.join(b'\x00' + bytes(c for p in row for c in p) for row in px)
    def chunk(t, b):
        return struct.pack('>I', len(b)) + t + b + struct.pack('>I', zlib.crc32(t + b))
    out = b'\x89PNG\r\n\x1a\n' + chunk(b'IHDR', struct.pack('>IIBBBBB', w, h, 8, 6, 0, 0, 0))
    out += chunk(b'IDAT', zlib.compress(raw, 9)) + chunk(b'IEND', b'')
    open(path, 'wb').write(out)
