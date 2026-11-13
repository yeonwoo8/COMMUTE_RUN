def C(h): return (int(h[1:3], 16), int(h[3:5], 16), int(h[5:7], 16), 255)

CLEAR = (0, 0, 0, 0)
OUT = C('#1c1230')
RIM = C('#fff4e2')

GLYPHS = {
    'A': ['.###.', '#...#', '#...#', '#####', '#...#', '#...#', '#...#'],
    'B': ['####.', '#...#', '#...#', '####.', '#...#', '#...#', '####.'],
    'C': ['.###.', '#...#', '#....', '#....', '#....', '#...#', '.###.'],
    'D': ['####.', '#...#', '#...#', '#...#', '#...#', '#...#', '####.'],
    'E': ['#####', '#....', '#....', '####.', '#....', '#....', '#####'],
    'F': ['#####', '#....', '#....', '####.', '#....', '#....', '#....'],
    'G': ['.###.', '#...#', '#....', '#.###', '#...#', '#...#', '.####'],
    'H': ['#...#', '#...#', '#...#', '#####', '#...#', '#...#', '#...#'],
    'I': ['###', '.#.', '.#.', '.#.', '.#.', '.#.', '###'],
    'J': ['..###', '...#.', '...#.', '...#.', '#..#.', '#..#.', '.##..'],
    'K': ['#...#', '#..#.', '#.#..', '##...', '#.#..', '#..#.', '#...#'],
    'L': ['#....', '#....', '#....', '#....', '#....', '#....', '#####'],
    'M': ['#...#', '##.##', '#.#.#', '#.#.#', '#...#', '#...#', '#...#'],
    'N': ['#...#', '##..#', '#.#.#', '#..##', '#...#', '#...#', '#...#'],
    'O': ['.###.', '#...#', '#...#', '#...#', '#...#', '#...#', '.###.'],
    'P': ['####.', '#...#', '#...#', '####.', '#....', '#....', '#....'],
    'Q': ['.###.', '#...#', '#...#', '#...#', '#.#.#', '#..#.', '.##.#'],
    'R': ['####.', '#...#', '#...#', '####.', '#.#..', '#..#.', '#...#'],
    'S': ['.####', '#....', '#....', '.###.', '....#', '....#', '####.'],
    'T': ['#####', '..#..', '..#..', '..#..', '..#..', '..#..', '..#..'],
    'U': ['#...#', '#...#', '#...#', '#...#', '#...#', '#...#', '.###.'],
    'V': ['#...#', '#...#', '#...#', '#...#', '#...#', '.#.#.', '..#..'],
    'W': ['#...#', '#...#', '#...#', '#.#.#', '#.#.#', '##.##', '#...#'],
    'X': ['#...#', '#...#', '.#.#.', '..#..', '.#.#.', '#...#', '#...#'],
    'Y': ['#...#', '#...#', '.#.#.', '..#..', '..#..', '..#..', '..#..'],
    'Z': ['#####', '....#', '...#.', '..#..', '.#...', '#....', '#####'],
    '0': ['.###.', '#...#', '#..##', '#.#.#', '##..#', '#...#', '.###.'],
    '1': ['..#..', '.##..', '..#..', '..#..', '..#..', '..#..', '.###.'],
    '2': ['.###.', '#...#', '....#', '...#.', '..#..', '.#...', '#####'],
    '3': ['#####', '...#.', '..#..', '...#.', '....#', '#...#', '.###.'],
    '4': ['...#.', '..##.', '.#.#.', '#..#.', '#####', '...#.', '...#.'],
    '5': ['#####', '#....', '####.', '....#', '....#', '#...#', '.###.'],
    '6': ['..##.', '.#...', '#....', '####.', '#...#', '#...#', '.###.'],
    '7': ['#####', '....#', '...#.', '..#..', '.#...', '.#...', '.#...'],
    '8': ['.###.', '#...#', '#...#', '.###.', '#...#', '#...#', '.###.'],
    '9': ['.###.', '#...#', '#...#', '.####', '....#', '...#.', '.##..'],
    '!': ['#', '#', '#', '#', '#', '.', '#'],
    '.': ['.', '.', '.', '.', '.', '.', '#'],
    '/': ['....#', '....#', '...#.', '..#..', '.#...', '#....', '#....'],
    '$': ['#...#', '#...#', '#####', '#.#.#', '#####', '.#.#.', '.#.#.'],  # won sign
}


def glyph_width(ch):
    return len(GLYPHS[ch][0])


def text_cells(text, scale, gap):
    """Yields (x, y, glyph_row) for each filled pixel of text at origin 0,0."""
    x = 0
    for ch in text:
        for gy, row in enumerate(GLYPHS[ch]):
            for gx, v in enumerate(row):
                if v != '#': continue
                for dy in range(scale):
                    for dx in range(scale):
                        yield x + gx * scale + dx, gy * scale + dy, gy
        x += glyph_width(ch) * scale + gap


def text_width(text, scale, gap):
    return sum(glyph_width(ch) * scale + gap for ch in text) - gap


# Styles: (light, fill, shade, extrude, extrude-dark)
YELLOW = (C('#fff4e2'), C('#ffd257'), C('#ffb86b'), C('#d8574a'), C('#824941'))
NEON = (C('#d8fbff'), C('#49e8ff'), C('#2fb0d8'), C('#ff4fa3'), C('#8a2f6e'))
RED = (C('#ffd6cc'), C('#ff6a5a'), C('#d8574a'), C('#824941'), C('#4a2030'))
DIM = (C('#9a86c4'), C('#6f6a94'), C('#504c6b'), C('#2a1f47'), OUT)


def styled_words(words, scale, gap, space, depth, pad=3):
    """Logo-style text: highlight top edge, shaded bottom, extruded shadow, chunky dark outline.
    words: list of (text, style)."""
    cells = []
    x = pad
    for word, style in words:
        for ch in word:
            cells.append((x, ch, style)); x += glyph_width(ch) * scale + gap
        x += space - gap
    W = x - space + gap + depth + pad
    H = 7 * scale + depth + pad * 2
    fill = {}
    for ox, ch, style in cells:
        for gy, row in enumerate(GLYPHS[ch]):
            for gx, v in enumerate(row):
                if v != '#': continue
                for y in range(scale):
                    for xx in range(scale):
                        fill[(ox + gx * scale + xx, pad + gy * scale + y)] = (style, gy * scale + y)

    px = [[CLEAR] * W for _ in range(H)]
    extrude = {}
    for (x, y), (style, _) in fill.items():
        for d in range(1, depth + 1):
            extrude.setdefault((x + d, y + d), (style, d))
    for (x, y) in set(fill) | set(extrude):
        for dy in range(-2, 3):
            for dx in range(-2, 3):
                if abs(dx) + abs(dy) <= 3 and 0 <= x + dx < W and 0 <= y + dy < H:
                    px[y + dy][x + dx] = OUT
    for (x, y), (style, d) in extrude.items():
        px[y][x] = style[3] if d < depth else style[4]
    for (x, y), (style, ly) in fill.items():
        if (x, y - 1) not in fill: c = style[0]
        elif ly >= 5 * scale: c = style[2]
        else: c = style[1]
        px[y][x] = c
    return px


def styled(text, style, scale, depth, gap=None, space=None):
    gap = scale if gap is None else gap
    space = scale * 4 if space is None else space
    return styled_words([(w, style) for w in text.split(' ')], scale, gap, space, depth)


SIGN_PLATE = C('#2a1f47')
SIGN_PLATE_TOP = C('#3b2a56')
BOLT = C('#c9a9d6')


def _mix(a, b, t):
    return tuple(round(a[i] * (1 - t) + b[i] * t) for i in range(3)) + (255,)


def neon_sign(words, scale, frame, gap=None, space=None, pad=7):
    """Flat neon letters on a navy sign plate, like the 333 sign on the street.
    words: list of (text, color). The tube glows onto the plate instead of casting a 3D shadow."""
    gap = scale if gap is None else gap
    space = scale * 4 if space is None else space
    border = 3
    left = border + pad
    fill = {}
    x = left
    for word, color in words:
        for ch in word:
            for gy, row in enumerate(GLYPHS[ch]):
                for gx, v in enumerate(row):
                    if v != '#': continue
                    for dy in range(scale):
                        for dx in range(scale):
                            fill[(x + gx * scale + dx, left + gy * scale + dy)] = color
            x += glyph_width(ch) * scale + gap
        x += space - gap
    W = x - space + pad + border
    H = left * 2 + 7 * scale

    px = [[SIGN_PLATE] * W for _ in range(H)]
    for y in range(H):
        for x in range(W):
            edge = min(x, y, W - 1 - x, H - 1 - y)
            if edge == 0: px[y][x] = OUT
            elif edge == 1: px[y][x] = frame
            elif edge == 2: px[y][x] = OUT
            elif y == 3: px[y][x] = SIGN_PLATE_TOP
    for x, y in ((0, 0), (W - 1, 0), (0, H - 1), (W - 1, H - 1)):
        px[y][x] = CLEAR
    for x, y in ((5, 5), (W - 6, 5), (5, H - 6), (W - 6, H - 6)):
        px[y][x] = BOLT

    # glow: two soft rings of the tube color around each stroke
    for (fx, fy), color in fill.items():
        for dy in range(-2, 3):
            for dx in range(-2, 3):
                gx, gy = fx + dx, fy + dy
                if (gx, gy) in fill or not (3 <= gx < W - 3 and 3 <= gy < H - 3): continue
                strength = 0.38 if max(abs(dx), abs(dy)) == 1 else 0.16
                base = px[gy][gx]
                lifted = _mix(SIGN_PLATE, color, strength)
                if sum(lifted[:3]) > sum(base[:3]): px[gy][gx] = lifted
    for (fx, fy), color in fill.items():
        px[fy][fx] = _mix(color, RIM, 0.55) if (fx, fy - 1) not in fill else color
    return px


def rimmed(shape, pad=2):
    """Adds a dark 1px outline and a light 1px rim around non-clear pixels."""
    h, w = len(shape), len(shape[0])
    px = [[CLEAR] * (w + pad * 2) for _ in range(h + pad * 2)]
    for y in range(h):
        for x in range(w):
            px[y + pad][x + pad] = shape[y][x]
    for color in (OUT, RIM):
        solid = [(x, y) for y, r in enumerate(px) for x, p in enumerate(r) if p[3]]
        for x, y in solid:
            for dx, dy in ((1, 0), (-1, 0), (0, 1), (0, -1)):
                nx, ny = x + dx, y + dy
                if 0 <= ny < len(px) and 0 <= nx < len(px[0]) and not px[ny][nx][3]:
                    px[ny][nx] = color
    return px


def grid(rows, colors):
    return [[colors.get(ch, CLEAR) for ch in row] for row in rows]
