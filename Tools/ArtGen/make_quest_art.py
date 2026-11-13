import os, sys
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import png
from pixfont import C, CLEAR, OUT, rimmed

ART = sys.argv[1]
def save(name, px): png.write(os.path.join(ART, name + '.png'), len(px[0]), len(px), px)

GOLD = C('#ffd257'); GOLD_SHADE = C('#ffb86b'); GOLD_LIGHT = C('#fff4e2'); GOLD_LINE = C('#824941')
WOOD = C('#9a6230'); WOOD2 = C('#8a5a33'); WOOD3 = C('#6f4727'); WOODL = C('#b5763c'); PLUM = C('#3b2a56')


def shape(w, h, inside):
    return [[GOLD if inside(x, y) else CLEAR for x in range(w)] for y in range(h)]


def shade_and_outline(px, line):
    """Lights top edges, darkens bottom edges, then wraps the shape in a 1px outline."""
    h, w = len(px), len(px[0])
    solid = lambda x, y: 0 <= x < w and 0 <= y < h and px[y][x] == GOLD
    out = [row[:] for row in px]
    for y in range(h):
        for x in range(w):
            if not solid(x, y): continue
            if not solid(x, y - 1): out[y][x] = GOLD_LIGHT
            elif not solid(x, y + 1): out[y][x] = GOLD_SHADE
    padded = [[CLEAR] * (w + 2) for _ in range(h + 2)]
    for y in range(h):
        for x in range(w):
            padded[y + 1][x + 1] = out[y][x]
    for y in range(h + 2):
        for x in range(w + 2):
            if padded[y][x][3]: continue
            if any(0 <= y + dy < h + 2 and 0 <= x + dx < w + 2 and padded[y + dy][x + dx][3] and padded[y + dy][x + dx] != line
                   for dx, dy in ((1, 0), (-1, 0), (0, 1), (0, -1))):
                padded[y][x] = line
    return padded


# wide speech bubble for the two-bread request, stretched from the original bubble
w, h, bubble = png.read(os.path.join(ART, 'Bubble.png'))
cols = list(range(0, 12)) + [12] * 24 + list(range(12, w))
save('BubbleWide', [[row[c] for c in cols] for row in bubble])

# golden key: ring with a hole, shaft, two teeth
def key(x, y):
    ring = (x - 5) ** 2 + (y - 5) ** 2 <= 20 and (x - 5) ** 2 + (y - 5) ** 2 > 3
    shaft = 9 <= x <= 20 and 4 <= y <= 6
    teeth = (14 <= x <= 15 and 7 <= y <= 9) or (18 <= x <= 20 and 7 <= y <= 10)
    return ring or shaft or teeth
save('Key', shade_and_outline(shape(21, 11, key), GOLD_LINE))

# golden four-leaf clover: one heart leaf rotated four times, gaps on the diagonals, stem between two leaves
UP_LEAF = {0: [(5, 7), (9, 11)], 1: [(4, 12)], 2: [(4, 12)], 3: [(5, 11)], 4: [(6, 10)], 5: [(7, 9)], 6: [(8, 8)]}
leaf = {(x, y) for y, spans in UP_LEAF.items() for a, b in spans for x in range(a, b + 1)}
turns = [lambda x, y: (x, y), lambda x, y: (y, 16 - x), lambda x, y: (16 - y, x), lambda x, y: (x, 16 - y)]
cells = {t(x, y) for t in turns for x, y in leaf}
cells |= {(x, y) for x in range(17) for y in range(17) if abs(x - 8) + abs(y - 8) <= 2}
stem = {(11, 11), (12, 12), (13, 13), (14, 14), (14, 15), (15, 16)}
px = shade_and_outline(shape(17, 17, lambda x, y: (x, y) in cells | stem), GOLD_LINE)
veins = {t(8, y) for t in turns for y in range(1, 7)} | {(8, 8)}
for x, y in veins:
    px[y + 1][x + 1] = GOLD_SHADE
save('Clover', px)

# treasure chest, closed and open
def chest(open_):
    c = [[CLEAR] * 36 for _ in range(30)]
    def rect(x0, y0, x1, y1, col):
        for y in range(y0, y1):
            for x in range(x0, x1): c[y][x] = col
    rect(0, 13, 36, 30, WOOD2)
    rect(0, 13, 36, 15, WOODL)
    rect(0, 28, 36, 30, WOOD3)
    for bx in (5, 27):
        rect(bx, 13, bx + 4, 30, GOLD); rect(bx, 13, bx + 4, 14, GOLD_LIGHT); rect(bx + 3, 14, bx + 4, 30, GOLD_SHADE)
    if open_:
        rect(2, 0, 34, 6, WOOD3); rect(2, 0, 34, 1, WOOD2)
        rect(1, 6, 35, 13, PLUM)
        rect(3, 8, 33, 13, GOLD_SHADE); rect(5, 9, 31, 13, GOLD); rect(9, 10, 27, 12, GOLD_LIGHT)
    else:
        rect(1, 3, 35, 13, WOOD); rect(3, 1, 33, 3, WOOD)
        rect(1, 3, 35, 5, WOODL); rect(3, 1, 33, 2, WOODL)
        rect(1, 11, 35, 13, WOOD3)
        for bx in (5, 27):
            rect(bx, 1, bx + 4, 13, GOLD); rect(bx, 1, bx + 4, 2, GOLD_LIGHT)
        rect(14, 9, 22, 19, GOLD); rect(14, 9, 22, 10, GOLD_LIGHT); rect(21, 10, 22, 19, GOLD_SHADE)
        rect(17, 12, 19, 16, OUT); rect(16, 12, 20, 14, OUT)
    return rimmed(c)
save('ChestClosed', chest(False))
save('ChestOpen', chest(True))
