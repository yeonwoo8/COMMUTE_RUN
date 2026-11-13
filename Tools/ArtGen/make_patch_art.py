# Post-jam art patches: arcade sign text, rolling wheel frames, DAY hud.
# Run from anywhere: python Tools/ArtGen/make_patch_art.py
import os
import png

ART = os.path.join(os.path.dirname(__file__), '..', '..', 'Assets', 'Art')


def load(name): return png.read(os.path.join(ART, name + '.png'))[2]
def save(name, px): png.write(os.path.join(ART, name + '.png'), len(px[0]), len(px), px)
def C(h): return (int(h[1:3], 16), int(h[3:5], 16), int(h[5:7], 16), 255)


CLEAR = (0, 0, 0, 0)

# ---------- arcade sign: 333 -> 오락실 (Buildings_A.png, edited in place) ----------
SIGN_BG = C('#1c1230')
NEON = C('#49e8ff')
NEON_SHADOW = (43, 139, 153, 255)
SIGN_X0, SIGN_Y0, SIGN_X1, SIGN_Y1 = 317, 33, 422, 72  # inside the pink frame
BLOCK = 2

HANGUL = {
    '오': ['....#####....',
          '..##.....##..',
          '.#.........#.',
          '.#.........#.',
          '.#.........#.',
          '..##.....##..',
          '....#####....',
          '.............',
          '.............',
          '......#......',
          '......#......',
          '......#......',
          '......#......',
          '#############',
          '.............'],
    '락': ['########..#..',
          '.......#..#..',
          '.......#..#..',
          '########..###',
          '#.........#..',
          '#.........#..',
          '########..#..',
          '..........#..',
          '.###########.',
          '...........#.',
          '...........#.',
          '...........#.',
          '...........#.',
          '...........#.',
          '.............'],
    '실': ['....#.....#..',
          '....#.....#..',
          '...#.#....#..',
          '..#...#...#..',
          '.#.....#..#..',
          '#.......#.#..',
          '..........#..',
          '.............',
          '.###########.',
          '...........#.',
          '...........#.',
          '.###########.',
          '.#...........',
          '.#...........',
          '.###########.'],
}


def draw_glyph(px, rows, x0, y0, color, offset):
    for gy, row in enumerate(rows):
        for gx, cell in enumerate(row):
            if cell != '#':
                continue
            for dy in range(BLOCK):
                for dx in range(BLOCK):
                    px[y0 + offset + gy * BLOCK + dy][x0 + offset + gx * BLOCK + dx] = color


def arcade_sign():
    px = load('Buildings_A')
    for y in range(SIGN_Y0, SIGN_Y1 + 1):
        for x in range(SIGN_X0, SIGN_X1 + 1):
            px[y][x] = SIGN_BG

    word = '오락실'
    glyph_w, glyph_h, gap = 13 * BLOCK, 15 * BLOCK, 8
    total = len(word) * glyph_w + (len(word) - 1) * gap
    x = SIGN_X0 + (SIGN_X1 - SIGN_X0 + 1 - total - BLOCK) // 2
    y = SIGN_Y0 + (SIGN_Y1 - SIGN_Y0 + 1 - glyph_h - BLOCK) // 2
    for i, ch in enumerate(word):
        gx = x + i * (glyph_w + gap)
        draw_glyph(px, HANGUL[ch], gx, y, NEON_SHADOW, BLOCK)
        draw_glyph(px, HANGUL[ch], gx, y, NEON, 0)
    save('Buildings_A', px)


# ---------- rolling wheels: a spoke through the hub turns counterclockwise ----------
HUB = (207, 195, 198, 255)
BOLT = (138, 116, 112, 255)
WHEEL_FRAMES = 6
# Cells of the 6x6 hub grid (2px each) that ring the centre dot, counterclockwise from the top.
RING = [(1, 3), (1, 2), (1, 1), (2, 1), (3, 1), (4, 1)]


def wheel_frames(name, hubs):
    base = load(name)
    for frame in range(WHEEL_FRAMES):
        px = [row[:] for row in base]
        r, c = RING[frame]
        for hx, hy in hubs:
            for cr, cc in ((r, c), (5 - r, 5 - c)):
                for dy in range(BLOCK):
                    for dx in range(BLOCK):
                        x, y = hx + cc * BLOCK + dx, hy + cr * BLOCK + dy
                        if px[y][x] == HUB:
                            px[y][x] = BOLT
        save('%s_%d' % (name, frame), px)


# ---------- DAY hud: same lettering as HudLuck, panel with one digit slot ----------
HUD_TEXT = (255, 224, 138, 255)
DAY_GLYPHS = [
    ['#######..'] + ['##.....##'] * 6 + ['#######..'],
    ['..#####..'] + ['##.....##'] * 3 + ['#########'] + ['##.....##'] * 3,
    ['##....##'] * 3 + ['.##..##.', '..####..'] + ['...##...'] * 3,
]


def hud_day():
    w = 1 + sum(len(g[0]) for g in DAY_GLYPHS) + 3 * (len(DAY_GLYPHS) - 1) + 1
    px = [[CLEAR] * w for _ in range(18)]
    x = 1
    for glyph in DAY_GLYPHS:
        for gy, row in enumerate(glyph):
            for gx, cell in enumerate(row):
                if cell == '#':
                    for dy in range(2):
                        px[1 + gy * 2 + dy][x + gx] = HUD_TEXT
        x += len(glyph[0]) + 3
    save('HudDay', px)

    # Drop columns from the plain left area (DAY is shorter than LUCK) and the last two digit slots.
    panel = load('HudPanel')
    keep = list(range(0, 20)) + list(range(33, 71)) + list(range(95, 100))
    save('HudDayPanel', [[row[x] for x in keep] for row in panel])


arcade_sign()
wheel_frames('CarDetails', [(62, 54), (140, 54)])
wheel_frames('BusDetails', [(75, 62), (183, 62)])
hud_day()


# ---------- slot machine and sign: the chest's outline + light rim so they stand out ----------
def rim_like_chest(name):
    from pixfont import RIM, rimmed
    px = load(name)
    h, w = len(px), len(px[0])
    edge = [px[y][x] for y in range(h) for x in range(w) if px[y][x][3] and
            any(not (0 <= x + dx < w and 0 <= y + dy < h) or not px[y + dy][x + dx][3]
                for dx, dy in ((1, 0), (-1, 0), (0, 1), (0, -1)))]
    if all(p == RIM for p in edge):
        return  # already rimmed
    save(name, rimmed(px))


rim_like_chest('Slot')
rim_like_chest('Sign')


# ---------- fortune teller: hooded, facing the player, crystal ball dim (idle) or lit (reading) ----------
TELLER = [
    '......hhhhh.......',
    '.....hHHHHHh......',
    '....hHHHHHHHh.....',
    '...hHHHHHHHHHh....',
    '...HHssssHHHHH....',
    '...HsesssHHHHHH...',
    '...HsssssHHHHHH...',
    '...HHsssHHHHHHH...',
    '.*..HggggHHHHHHH..',
    '...*RRRRRRRRRRRR..',
    '.bbRRRRRRRRRRRRR..',
    'bwbbRRRRRRRRRRRR..',
    'bbbbRRRRRRRRRRRR..',
    '.bbRRRRRyyyyRRRRR.',
    'ssssRRRRRRRyRRRRR.',
    '.*.RRRRRRyyyRRRRR.',
    '...RRRRRRRRyRRRRR.',
    '..RRRRRRyyyyRRRRR.',
    '..RRRRRRRRRRRRRRRR',
    '..RRRRRRRRRRRRRRRR',
    '.RRRRRRRRRRRRRRRRR',
    '.rRRRRRRRRRRRRRRRR',
    '.rrrrrrrrrrrrrrrrr',
    '....ff......ff....',
    '...fff.....fff....',
]


def fortune_teller(lit):
    colors = {
        'h': C('#7b5cc0'), 'H': C('#3b2a56'), 'R': C('#5b3f8c'), 'r': C('#2a1f47'),
        's': C('#f6c9a8'), 'e': C('#1c1230'), 'g': C('#ffd257'), 'y': C('#ffd257'), 'f': C('#1c1230'),
        'b': C('#49e8ff') if lit else C('#5e7fb8'),
        'w': C('#fff4e2') if lit else C('#a2cde4'),
        '*': C('#fff4e2') if lit else CLEAR,
    }
    px = []
    for row in TELLER:
        line = []
        for cell in row:
            line += [colors.get(cell, CLEAR)] * BLOCK
        px += [line[:], line[:]]
    from pixfont import rimmed
    return rimmed(px)


save('FortuneTeller', fortune_teller(False))
save('FortuneTellerLit', fortune_teller(True))


# ---------- fortune cards: devil (minus), blank (zero), lucky (plus), and a white glow tinted in code ----------
CARD_SYMBOLS = {
    'Devil': ['.........', '.h.....h.', '.hh...hh.', '..hRRRh..', '.RRRRRRR.', '.RyRRRyR.',
              '.RRRRRRR.', '.RR.R.RR.', '..RRRRR..', '...R.R...', '.........', '.........'],
    'Blank': ['.........', '.........', '.........', '...ooo...', '..o...o..', '..o...o..',
              '..o...o..', '..o...o..', '...ooo...', '.........', '.........', '.........'],
    'Lucky': ['s.......s', '....S....', '....S....', '...SSS...', 'SSSSWSSSS', '.SSWWWSS.',
              '..SSSSS..', '..SS.SS..', '.SS...SS.', '.S.....S.', '.........', 's.......s'],
}
CARD_COLORS = {
    'Devil': {'F': C('#824941'), 'i': C('#1c1230'), 'h': C('#ff4fa3'), 'R': C('#d8574a'), 'y': C('#ffd257')},
    'Blank': {'F': C('#c9c9c0'), 'i': C('#efdcb8'), 'o': C('#a28e70')},
    'Lucky': {'F': C('#ffd257'), 'i': C('#2a1f47'), 'S': C('#ffd257'), 'W': C('#fff4e2'), 's': C('#49e8ff')},
}


def fortune_card(kind):
    from pixfont import rimmed
    rows = ['F' * 11] + ['F' + 'i' * 9 + 'F'] * 14 + ['F' * 11]
    rows = [list(r) for r in rows]
    for y, line in enumerate(CARD_SYMBOLS[kind]):
        for x, cell in enumerate(line):
            if cell != '.':
                rows[2 + y][1 + x] = cell
    colors = CARD_COLORS[kind]
    px = []
    for row in rows:
        line = []
        for cell in row:
            line += [colors[cell]] * BLOCK
        px += [line[:], line[:]]
    return rimmed(px)


def card_glow(w, h, ring):
    # White halo with alpha stepping down outwards; Image.color tints it per card.
    alphas = [255, 170, 100, 50]
    px = [[CLEAR] * w for _ in range(h)]
    for y in range(h):
        for x in range(w):
            d = max(ring - x, x - (w - 1 - ring), ring - y, y - (h - 1 - ring), 0)
            corner = min(x, w - 1 - x) + min(y, h - 1 - y) < ring - 1
            if d < len(alphas) and not corner:
                px[y][x] = (255, 255, 255, alphas[d])
    return px


for kind in CARD_SYMBOLS:
    save('Card' + kind, fortune_card(kind))
save('CardGlow', card_glow(26 + 8, 36 + 8, 4))

