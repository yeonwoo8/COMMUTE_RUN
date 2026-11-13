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
