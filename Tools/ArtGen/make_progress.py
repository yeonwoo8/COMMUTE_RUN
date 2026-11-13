import os, sys
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import png

OUT_DIR = sys.argv[1]
def C(h): return (int(h[1:3], 16), int(h[3:5], 16), int(h[5:7], 16), 255)
CLEAR = (0, 0, 0, 0)
OUT = C('#1c1230'); RIM = C('#fff4e2'); NAVY = C('#2a1f47'); PLUM = C('#3b2a56')
FRAME = C('#4a3f6e'); FRAME_HI = C('#6f6a94'); FRAME_LO = C('#2a1f47')
YELLOW = C('#ffd257'); YELLOW_HI = C('#ffe9a0'); ORANGE = C('#ffb86b'); RED = C('#d8574a')
HAIR = C('#3a2340'); SKIN = C('#f6c7ad'); SKIN2 = C('#dda58f'); EYE = C('#2a1a34'); BLUSH = C('#ff9a9a')
SHIRT = C('#2f7be0'); COLLAR = C('#fbf1ea'); TIE = C('#ff4f6a')
WALL = C('#6f6a94'); WALL_HI = C('#9a86c4'); WINDOW = C('#ffb86b'); CYAN = C('#49e8ff')
CREAM = C('#f6dcc4'); CREAM2 = C('#c9a9d6'); ROOF = C('#d8574a'); ROOF2 = C('#b5655a'); WOOD = C('#8a5a33')

BAR_W, BAR_H, INSET = 280, 16, 4
LANE_W, LANE_H = BAR_W - INSET * 2, BAR_H - INSET * 2


def save(name, px):
    png.write(os.path.join(OUT_DIR, name), len(px[0]), len(px), px)


def rimmed(shape, pad=2):
    """Draws dark 1px outline then a light 1px rim around non-clear pixels."""
    h, w = len(shape), len(shape[0])
    px = [[CLEAR] * (w + pad * 2) for _ in range(h + pad * 2)]
    for y in range(h):
        for x in range(w):
            px[y + pad][x + pad] = shape[y][x]
    for color in (OUT, RIM):
        solid = {(x, y) for y, r in enumerate(px) for x, p in enumerate(r) if p[3]}
        for x, y in solid:
            for dx, dy in ((1, 0), (-1, 0), (0, 1), (0, -1)):
                nx, ny = x + dx, y + dy
                if 0 <= ny < len(px) and 0 <= nx < len(px[0]) and not px[ny][nx][3]:
                    px[ny][nx] = color
    return px


# bar frame
bar = [[CLEAR] * BAR_W for _ in range(BAR_H)]
for y in range(BAR_H):
    for x in range(BAR_W):
        corner = (x in (0, BAR_W - 1)) and (y in (0, BAR_H - 1))
        if corner: continue
        edge = x in (0, BAR_W - 1) or y in (0, BAR_H - 1)
        if edge: c = OUT
        elif y == 1: c = FRAME_HI
        elif y == BAR_H - 2: c = FRAME_LO
        elif x < INSET - 1 or x >= BAR_W - INSET + 1 or y < INSET - 1 or y >= BAR_H - INSET + 1: c = FRAME
        elif x < INSET or x >= BAR_W - INSET or y < INSET or y >= BAR_H - INSET: c = OUT
        else:
            lx = x - INSET
            c = NAVY
            if y - INSET in (3, 4) and lx % 12 < 6: c = PLUM
            if lx in (LANE_W // 4, LANE_W // 2, LANE_W * 3 // 4): c = FRAME_HI
        bar[y][x] = c
save('TrackBar.png', bar)

# fill (lane sized), candy stripes
fill = []
for y in range(LANE_H):
    row = []
    for x in range(LANE_W):
        if y == 0: c = YELLOW_HI
        elif y >= LANE_H - 2: c = ORANGE
        else: c = YELLOW_HI if (x + y) % 10 < 3 else YELLOW
        row.append(c)
    fill.append(row)
save('TrackFill.png', fill)

# player face
FACE = [
    '....hhhhhh....',
    '..hhhhhhhhhh..',
    '.hhhhhhhhhhhh.',
    '.hhhhhhhhhhhh.',
    'hhhssshhhssshh',
    'hhssssssssssh.',
    'hhssssssssssh.',
    '.sseesssseess.',
    '.sseesssseess.',
    '.sbssssssssbs.',
    '.ssssssmmssss.',
    '..ssssssssss..',
    '...dssssssd...',
    '..wwwwrrwwww..',
    '.uuuwwrrwwuuu.',
]
colors = {'h': HAIR, 's': SKIN, 'e': EYE, 'b': BLUSH, 'm': SKIN2, 'd': SKIN2, 'w': COLLAR, 'r': TIE, 'u': SHIRT}
save('TrackPlayer.png', rimmed([[colors.get(ch, CLEAR) for ch in row] for row in FACE]))

# office tower with neon 3 sign
OFFICE = [
    '.....nnnnn.....',
    '.....n333n.....',
    '.....nnnnn.....',
    '......lll......',
    '.lllllwwwwwwww.',
    '.lwwwwwwwwwwww.',
    '.lwgwgwgwgwgww.',
    '.lwgwgwgwgwgww.',
    '.lwwwwwwwwwwww.',
    '.lwgwgwgwgwgww.',
    '.lwgwgwgwgwgww.',
    '.lwwwwwwwwwwww.',
    '.lwgwgwgwgwgww.',
    '.lwgwgwgwgwgww.',
    '.lwwwwwwwwwwww.',
    '.lwwwwwddwwwww.',
    '.lwwwwwddwwwww.',
    '.lwwwwwddwwwww.',
]
colors = {'n': OUT, '3': CYAN, 'l': WALL_HI, 'w': WALL, 'g': WINDOW, 'd': NAVY}
save('TrackOffice.png', rimmed([[colors.get(ch, CLEAR) for ch in row] for row in OFFICE]))

# home
HOME = [
    '......rr......',
    '.....rrrr.....',
    '....rrRRrr....',
    '...rrRRRRrr...',
    '..rrRRRRRRrr..',
    '.rrrrrrrrrrrr.',
    '..cccccccccc..',
    '..cggcccddcc..',
    '..cggcccddcc..',
    '..cccccddddc..',
    '..cccccddddc..',
    '..kkkkkkkkkk..',
]
colors = {'r': ROOF, 'R': ROOF2, 'c': CREAM, 'g': WINDOW, 'd': WOOD, 'k': CREAM2}
save('TrackHome.png', rimmed([[colors.get(ch, CLEAR) for ch in row] for row in HOME]))
