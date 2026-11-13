import math, os, sys
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import png
from pixfont import C, CLEAR, OUT, GLYPHS, YELLOW, NEON, DIM, styled, neon_sign, rimmed, grid, text_cells

OUT_DIR = sys.argv[1]
def save(name, px): png.write(os.path.join(OUT_DIR, name + '.png'), len(px[0]), len(px), px)

NAVY = C('#2a1f47'); PLUM = C('#3b2a56'); DPURP = C('#4a3f6e'); SLATE = C('#504c6b'); DUSK = C('#6f6a94')
WALL2 = C('#9a86c4'); LILAC = C('#c9a9d6'); CREAM = C('#fbf1ea'); PAPER = C('#f6dcc4'); PEACH = C('#ffd3a8')
ORANGE = C('#ffb86b'); PINK = C('#f2b6c6'); SKY = C('#b9a6e0'); HOTPINK = C('#ff4fa3'); CYAN = C('#49e8ff')
RED_C = C('#d8574a'); RED2 = C('#b5655a'); RED3 = C('#824941'); YEL = C('#ffd257'); LIGHT = C('#fff4e2')
WOOD = C('#9a6230'); WOOD2 = C('#8a5a33'); WOOD3 = C('#6f4727'); WOODL = C('#b5763c')
GREEN = C('#5c857d'); GREEN2 = C('#4c7069'); MINT = C('#7fb8ad'); GRAY = C('#9c8f9e'); GRAY2 = C('#bcb7ab')
SKIN = C('#f6c7ad'); SKIN2 = C('#dda58f'); ANGRY = C('#ff9a86'); ANGRY2 = C('#e8705e'); EYE = C('#2a1a34')
SUIT = C('#504c6b'); SUIT2 = C('#3b3656'); SHIRT = C('#fbf1ea'); TIE = C('#d8574a'); BLUSH = C('#ff9a9a')


class Canvas:
    def __init__(self, w, h, fill=CLEAR):
        self.w, self.h = w, h
        self.px = [[fill] * w for _ in range(h)]

    def put(self, x, y, c):
        if 0 <= x < self.w and 0 <= y < self.h: self.px[y][x] = c

    def rect(self, x0, y0, x1, y1, c):
        for y in range(max(0, y0), min(self.h, y1)):
            for x in range(max(0, x0), min(self.w, x1)):
                self.px[y][x] = c

    def box(self, x0, y0, x1, y1, fill, line=OUT):
        self.rect(x0, y0, x1, y1, line); self.rect(x0 + 1, y0 + 1, x1 - 1, y1 - 1, fill)

    def disc(self, cx, cy, r, c):
        for y in range(cy - r, cy + r + 1):
            for x in range(cx - r, cx + r + 1):
                if (x - cx) ** 2 + (y - cy) ** 2 <= r * r + r: self.put(x, y, c)

    def blend(self, x0, y0, x1, y1, c, a):
        for y in range(max(0, y0), min(self.h, y1)):
            for x in range(max(0, x0), min(self.w, x1)):
                p = self.px[y][x]
                self.px[y][x] = tuple(round(p[i] * (1 - a) + c[i] * a) for i in range(3)) + (255,)

    def text(self, s, x, y, c, scale=1):
        for tx, ty, _ in text_cells(s, scale, scale):
            self.put(x + tx, y + ty, c)


# ---------- office background (desk and boss are separate layers) ----------
o = Canvas(640, 360, C('#7f74ad'))
for x in range(0, 640, 32): o.rect(x, 0, x + 2, 236, C('#8a7fb8'))
o.rect(0, 0, 640, 6, LILAC); o.rect(0, 6, 640, 8, DUSK); o.rect(0, 8, 640, 9, DPURP)
o.rect(0, 226, 640, 229, LILAC); o.rect(0, 229, 640, 235, DPURP); o.rect(0, 235, 640, 236, OUT)
# carpet tiles
for y in range(236, 360):
    for x in range(640):
        o.px[y][x] = SLATE if ((x // 40) + (y - 236) // 20) % 2 == 0 else C('#585273')
for y in range(236, 360, 20): o.rect(0, y, 640, y + 1, DPURP)
o.rect(0, 236, 640, 240, DPURP)

# big window with skyline and a neon 333 tower
o.box(28, 34, 316, 206, PAPER)
o.rect(29, 202, 315, 205, LILAC)
bands = [(56, SKY), (82, C('#d6aedd')), (112, PINK), (150, PEACH), (196, ORANGE)]
y0 = 44
for y1, c in bands:
    o.rect(38, y0, 306, y1, c); y0 = y1
for x0, top, w in [(38, 132, 26), (64, 112, 22), (86, 140, 30), (150, 120, 24), (174, 100, 20), (194, 136, 34),
                   (228, 116, 26), (254, 140, 30), (284, 124, 22)]:
    o.rect(x0, top, x0 + w, 196, DUSK)
    for wy in range(top + 5, 190, 9):
        for wx in range(x0 + 4, x0 + w - 4, 6):
            if (wx * 5 + wy * 7) % 7 < 3: o.rect(wx, wy, wx + 3, wy + 4, ORANGE)
o.rect(116, 84, 150, 196, PLUM)
o.box(110, 70, 156, 92, NAVY, HOTPINK)
for d in range(3):
    o.text('3', 117 + d * 12, 74, CYAN, 2)
for wy in range(100, 190, 10):
    for wx in (122, 132, 142):
        o.rect(wx, wy, wx + 4, wy + 5, ORANGE)
o.rect(38, 180, 306, 196, SLATE)
o.rect(170, 44, 174, 196, PAPER); o.rect(38, 118, 306, 122, PAPER)
o.rect(174, 44, 175, 196, LILAC); o.rect(38, 122, 306, 123, LILAC)
o.box(22, 204, 322, 212, PAPER); o.rect(23, 210, 321, 211, LILAC)
# blinds hanging at top of window
for x in range(30, 314, 4): o.rect(x, 36, x + 3, 50, CREAM)
o.rect(28, 50, 316, 52, GRAY2)

# whiteboard with a rising luck chart behind the boss
o.box(380, 40, 612, 150, CREAM)
o.rect(381, 41, 611, 44, LIGHT)
o.rect(392, 132, 600, 134, DUSK); o.rect(392, 60, 394, 134, DUSK)
for i, hgt in enumerate([14, 22, 18, 34, 30, 48, 60]):
    x = 404 + i * 26
    o.rect(x, 132 - hgt, x + 14, 132, YEL); o.rect(x, 132 - hgt, x + 14, 133 - hgt, LIGHT)
    o.rect(x + 12, 132 - hgt, x + 14, 132, ORANGE)
o.text('LUCK', 404, 52, RED_C, 2)
o.text('333', 540, 52, RED_C, 2)
o.box(470, 148, 524, 154, GRAY2)

# wall clock, plant, filing cabinet
o.disc(350, 64, 14, OUT); o.disc(350, 64, 12, DPURP); o.disc(350, 64, 10, CREAM)
o.rect(349, 56, 351, 65, OUT); o.rect(343, 63, 351, 65, OUT); o.put(350, 64, RED_C)
o.box(328, 150, 372, 236, GRAY2); o.rect(329, 151, 371, 153, CREAM)
for y in (160, 186, 212):
    o.box(332, y, 368, y + 22, C('#a39e92')); o.box(343, y + 8, 357, y + 12, DUSK)
for cx, cy, r, c in [(612, 186, 13, GREEN2), (598, 200, 11, GREEN), (626, 202, 11, GREEN), (612, 172, 9, MINT)]:
    o.disc(cx, cy, r + 1, OUT)
for cx, cy, r, c in [(612, 186, 13, GREEN2), (598, 200, 11, GREEN), (626, 202, 11, GREEN), (612, 172, 9, MINT)]:
    o.disc(cx, cy, r, c)
o.box(596, 208, 628, 248, RED2); o.rect(597, 209, 627, 213, RED_C); o.rect(597, 238, 627, 247, RED3)
o.blend(590, 248, 634, 254, OUT, 0.3)
o.blend(330, 236, 374, 242, OUT, 0.3)
save('Office', o.px)

# ---------- desk (drawn over the boss) ----------
d = Canvas(260, 76)
d.box(0, 0, 260, 14, WOOD)
d.rect(1, 1, 259, 3, WOODL); d.rect(1, 12, 259, 13, WOOD3)
d.box(6, 13, 254, 76, WOOD2)
d.box(14, 22, 120, 68, WOOD); d.box(140, 22, 246, 68, WOOD)
d.rect(15, 23, 119, 25, WOODL); d.rect(141, 23, 245, 25, WOODL)
d.box(52, 40, 82, 46, YEL); d.box(178, 40, 208, 46, YEL)
d.box(96, 2, 164, 12, YEL); d.text('BOSS', 117, 3, OUT)
save('OfficeDesk', d.px)

# ---------- boss: 2 frames angry, 2 frames happy ----------
def boss(mood, frame):
    b = Canvas(40, 44)
    face, face2 = (ANGRY, ANGRY2) if mood == 'angry' else (SKIN, SKIN2)
    shake = (frame if mood == 'angry' else 0)
    lift = (0 if frame == 0 else -1) if mood == 'happy' else 0
    ox = 0 + shake
    # body
    b.rect(ox + 4, 28 + lift, ox + 36, 44, SUIT)
    b.rect(ox + 4, 28 + lift, ox + 8, 44, SUIT2); b.rect(ox + 32, 28 + lift, ox + 36, 44, SUIT2)
    b.rect(ox + 16, 28 + lift, ox + 24, 44, SHIRT)
    b.rect(ox + 19, 29 + lift, ox + 21, 42, TIE); b.rect(ox + 18, 29 + lift, ox + 22, 31 + lift, TIE)
    b.rect(ox + 14, 28 + lift, ox + 16, 36 + lift, SUIT2); b.rect(ox + 24, 28 + lift, ox + 26, 36 + lift, SUIT2)
    # neck + head
    b.rect(ox + 17, 25 + lift, ox + 23, 29 + lift, face2)
    b.rect(ox + 11, 6 + lift, ox + 29, 26 + lift, face)
    b.rect(ox + 12, 5 + lift, ox + 28, 6 + lift, face)
    b.rect(ox + 11, 24 + lift, ox + 29, 26 + lift, face2)
    # gray side hair, bald top shine
    b.rect(ox + 10, 9 + lift, ox + 13, 19 + lift, GRAY); b.rect(ox + 27, 9 + lift, ox + 30, 19 + lift, GRAY)
    b.rect(ox + 15, 7 + lift, ox + 19, 8 + lift, LIGHT)
    # ears
    b.rect(ox + 9, 13 + lift, ox + 11, 18 + lift, face2); b.rect(ox + 29, 13 + lift, ox + 31, 18 + lift, face2)
    # glasses
    b.rect(ox + 13, 13 + lift, ox + 19, 17 + lift, OUT); b.rect(ox + 21, 13 + lift, ox + 27, 17 + lift, OUT)
    b.rect(ox + 14, 14 + lift, ox + 18, 16 + lift, CREAM); b.rect(ox + 22, 14 + lift, ox + 26, 16 + lift, CREAM)
    b.rect(ox + 19, 14 + lift, ox + 21, 15 + lift, OUT)
    # mustache
    b.rect(ox + 15, 19 + lift, ox + 25, 21 + lift, GRAY); b.rect(ox + 14, 20 + lift, ox + 16, 22 + lift, GRAY)
    b.rect(ox + 24, 20 + lift, ox + 26, 22 + lift, GRAY)
    if mood == 'angry':
        b.rect(ox + 15, 15, ox + 18, 16, EYE); b.rect(ox + 22, 15, ox + 25, 16, EYE)
        b.put(ox + 13, 11, OUT); b.rect(ox + 14, 12, ox + 18, 13, OUT); b.rect(ox + 22, 12, ox + 26, 13, OUT); b.put(ox + 26, 11, OUT)
        mouth_h = 3 if frame == 0 else 2
        b.rect(ox + 16, 22, ox + 24, 22 + mouth_h, OUT); b.rect(ox + 17, 22 + mouth_h - 1, ox + 23, 22 + mouth_h, RED3)
        # anger vein
        vx, vy = ox + 25, 4 - frame
        for x, y in [(0, 1), (1, 0), (2, 1), (1, 2), (1, 1)]:
            b.put(vx + x, vy + y, RED_C)
        b.put(vx + 3, vy, RED_C); b.put(vx + 3, vy + 2, RED_C)
        # fist pounding the desk
        fy = 36 if frame == 0 else 40
        b.rect(ox + 30, fy, ox + 37, fy + 5, face); b.rect(ox + 30, fy + 4, ox + 37, fy + 5, face2)
    else:
        for x in (15, 16, 17): b.put(ox + x, 15 + lift - (1 if x == 16 else 0), EYE)
        for x in (23, 24, 25): b.put(ox + x, 15 + lift - (1 if x == 24 else 0), EYE)
        b.rect(ox + 12, 18 + lift, ox + 14, 19 + lift, BLUSH); b.rect(ox + 26, 18 + lift, ox + 28, 19 + lift, BLUSH)
        b.rect(ox + 17, 22 + lift, ox + 23, 23 + lift, OUT); b.put(ox + 16, 21 + lift, OUT); b.put(ox + 23, 21 + lift, OUT)
        # hand raising a bonus envelope
        ey = 16 + lift * 2
        b.rect(ox + 31, ey + 8, ox + 36, ey + 22, SUIT2)
        b.rect(ox + 30, ey + 4, ox + 37, ey + 9, SKIN)
        b.rect(ox + 28, ey - 6, ox + 40, ey + 3, YEL); b.rect(ox + 28, ey - 6, ox + 40, ey - 5, LIGHT)
        b.rect(ox + 28, ey + 2, ox + 40, ey + 3, ORANGE)
        b.put(ox + 33, ey - 3, RED_C); b.put(ox + 34, ey - 3, RED_C); b.put(ox + 33, ey - 2, RED_C); b.put(ox + 34, ey - 2, RED_C)
    return rimmed(b.px)

for mood in ('angry', 'happy'):
    for f in range(2):
        save('Boss_%s_%d' % (mood.capitalize(), f), boss(mood, f))

# ---------- text sprites ----------
save('QuotaClear', neon_sign([('QUOTA', C('#ffd257')), ('CLEAR!', C('#ffd257'))], 3, CYAN))
save('QuotaFail', neon_sign([('QUOTA', C('#ff6a5a')), ('FAILED...', C('#ff6a5a'))], 3, HOTPINK))
save('DayLabel', styled('DAY', NEON, 6, 4))
for i in range(10):
    save('DayDigit_%d' % i, styled(str(i), NEON, 6, 4))
    save('PayDigit_%d' % i, styled(str(i), YELLOW, 3, 2))
save('PayWon', styled('$', YELLOW, 3, 2))
save('ComboX2', styled('X2', YELLOW, 2, 2))
save('ComboX3', styled('X3', NEON, 2, 2))
save('ComboPipOn', styled('3', YELLOW, 2, 2))
save('ComboPipOff', styled('3', DIM, 2, 2))
save('Fever', styled('333 FEVER!', NEON, 4, 3))

# ---------- TV quota digits (3x5 blocks like the old 333) ----------
TV = {
    '0': ['###', '#.#', '#.#', '#.#', '###'], '1': ['.#.', '##.', '.#.', '.#.', '###'],
    '2': ['###', '..#', '###', '#..', '###'], '3': ['###', '..#', '.##', '..#', '###'],
    '4': ['#.#', '#.#', '###', '..#', '..#'], '5': ['###', '#..', '###', '..#', '###'],
    '6': ['###', '#..', '###', '#.#', '###'], '7': ['###', '..#', '..#', '.#.', '.#.'],
    '8': ['###', '#.#', '###', '#.#', '###'], '9': ['###', '#.#', '###', '..#', '###'],
}
for ch, rows in TV.items():
    t = Canvas(15, 25)
    for gy, row in enumerate(rows):
        for gx, v in enumerate(row):
            if v == '#': t.rect(gx * 5, gy * 5, gx * 5 + 5, gy * 5 + 5, CYAN)
    for x in range(15):
        if t.px[0][x][3]: t.px[0][x] = C('#d8fbff')
    save('TvDigit_' + ch, t.px)

# ---------- magpie, wings up / down (flies right to left) ----------
MAGPIE = [
    ["..........bb........",
     ".........bbbb.......",
     "........bwwbb.......",
     ".......bwwwb........",
     "..kk..bbwwb.........",
     ".kwkkbbbbk..........",
     "kkkkkkkkkkkkkk......",
     "..kkkkwwwwwkkkkkk...",
     "...kkwwwwwwwkk.kkkkk",
     "....kkkkkkkkk....kkk",
     ".....k..k...........",
     "....................",
     "...................."],
    ["....................",
     "....................",
     "....................",
     "....................",
     "..kk................",
     ".kwkk...............",
     "kkkkkkkkkkkkkk......",
     "..kkkkwwbbbbkkkkk...",
     "...kkwwwbwwbbk.kkkkk",
     "....kkkbwwwbbk...kkk",
     ".....k..bwwb........",
     ".........bbb........",
     "..........b........."],
]
for i, rows in enumerate(MAGPIE):
    save('Magpie_%d' % i, rimmed(grid(rows, {'k': NAVY, 'w': CREAM, 'b': C('#3a7bd5')})))
