import os, sys, math
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import png

W, H = 640, 360
def C(h): return (int(h[1:3], 16), int(h[3:5], 16), int(h[5:7], 16), 255)

OUT = C('#1c1230'); NAVY = C('#2a1f47'); PLUM = C('#3b2a56'); DPURP = C('#4a3f6e')
SLATE = C('#504c6b'); DUSK = C('#6f6a94'); WALL = C('#8a78bb'); WALL2 = C('#9a86c4')
LILAC = C('#c9a9d6'); CREAM = C('#fbf1ea'); PAPER = C('#f6dcc4'); PEACH = C('#ffd3a8')
ORANGE = C('#ffb86b'); PINK = C('#f2b6c6'); SKY = C('#b9a6e0'); HOTPINK = C('#ff4fa3')
RED = C('#d8574a'); RED2 = C('#b5655a'); RED3 = C('#824941'); CYAN = C('#49e8ff')
WOOD = C('#9a6230'); WOOD2 = C('#8a5a33'); WOOD3 = C('#6f4727'); WOODL = C('#b5763c')
BLUE = C('#2f7be0'); BLUE2 = C('#2460b8'); BLUE3 = C('#1f4f9e'); BLUEL = C('#6aa6f2')
GREEN = C('#5c857d'); GREEN2 = C('#4c7069'); MINT = C('#7fb8ad'); GRAY = C('#9c8f9e')
YELLOW = C('#ffd257')

px = [[WALL] * W for _ in range(H)]

def put(x, y, c):
    if 0 <= x < W and 0 <= y < H: px[y][x] = c

def rect(x0, y0, x1, y1, c):
    for y in range(max(0, y0), min(H, y1)):
        for x in range(max(0, x0), min(W, x1)):
            px[y][x] = c

def box(x0, y0, x1, y1, fill, line=OUT):
    rect(x0, y0, x1, y1, line)
    rect(x0 + 1, y0 + 1, x1 - 1, y1 - 1, fill)

def blend(x0, y0, x1, y1, c, a, inside=lambda x, y: True):
    for y in range(max(0, y0), min(H, y1)):
        for x in range(max(0, x0), min(W, x1)):
            if not inside(x, y): continue
            p = px[y][x]
            px[y][x] = tuple(round(p[i] * (1 - a) + c[i] * a) for i in range(3)) + (255,)

def disc(cx, cy, r, c):
    for y in range(cy - r, cy + r + 1):
        for x in range(cx - r, cx + r + 1):
            if (x - cx) ** 2 + (y - cy) ** 2 <= r * r + r: put(x, y, c)

# wall: striped wallpaper, chair rail, wainscot
for x in range(0, W, 24):
    rect(x, 0, x + 12, 190, WALL2)
for x in range(6, W, 24):
    for y in range(10, 190, 24):
        put(x + 6, y, LILAC); put(x + 5, y + 1, LILAC); put(x + 7, y + 1, LILAC); put(x + 6, y + 2, LILAC)
rect(0, 0, W, 6, LILAC); rect(0, 6, W, 8, DUSK); rect(0, 8, W, 9, DPURP)
rect(0, 188, W, 190, LILAC); rect(0, 190, W, 193, DUSK); rect(0, 193, W, 194, OUT)
rect(0, 194, W, 250, DUSK)
for x in range(8, W, 64):
    rect(x, 202, x + 48, 243, SLATE)
    rect(x + 1, 203, x + 48, 243, DUSK)
    rect(x + 1, 242, x + 48, 243, WALL)
rect(0, 250, W, 252, WALL); rect(0, 252, W, 259, DPURP); rect(0, 259, W, 260, OUT)

# floor planks
for y in range(260, H):
    row = (y - 260) // 14
    for x in range(W):
        px[y][x] = WOOD if row % 2 == 0 else WOOD2
for r, y in enumerate(range(260, H, 14)):
    rect(0, y, W, y + 1, WOOD3)
    rect(0, y + 1, W, y + 2, WOODL)
    off = (r * 53) % 96
    for x in range(off, W, 96):
        rect(x, y, x + 1, y + 14, WOOD3)
        rect(x + 1, y + 2, x + 2, y + 14, WOODL)
rect(0, 260, W, 263, WOOD3)

# window light on floor
blend(40, 263, 300, H, PEACH, 0.22, lambda x, y: 60 + (y - 263) * 0.9 <= x <= 210 + (y - 263) * 0.9)

# window
box(46, 36, 212, 176, PAPER)
rect(47, 172, 211, 175, LILAC)
view = (56, 46, 202, 166)
bands = [(46, SKY), (66, C('#d6aedd')), (86, PINK), (112, PEACH), (166, ORANGE)]
y0 = view[1]
for y1, c in bands:
    rect(view[0], y0, view[2], y1, c); y0 = y1
disc(150, 118, 12, C('#fff4e2'))
for x0, top, w in [(56, 120, 18), (74, 104, 14), (88, 128, 20), (108, 96, 16), (124, 116, 12), (170, 108, 18), (188, 124, 14)]:
    rect(x0, top, x0 + w, 166, DUSK)
    for wy in range(top + 4, 160, 8):
        for wx in range(x0 + 3, x0 + w - 3, 5):
            if (wx * 7 + wy * 3) % 5 < 2: rect(wx, wy, wx + 2, wy + 3, ORANGE)
rect(view[0], 150, view[2], 166, SLATE)
rect(127, 46, 131, 166, PAPER); rect(56, 104, 202, 108, PAPER)
rect(131, 46, 132, 166, LILAC); rect(56, 108, 202, 109, LILAC)
box(40, 174, 218, 182, PAPER); rect(41, 180, 217, 181, LILAC)

# curtains and rod
def curtain(x0, x1, flip):
    for y in range(34, 186):
        sway = int(3 * math.sin((y - 34) / 18))
        a, b = x0 + (sway if flip else 0), x1 + (0 if flip else sway)
        rect(a, y, b, y + 1, RED)
        for fx in range(a + 6, b - 2, 8):
            rect(fx, y, fx + 2, y + 1, RED2)
        put(a, y, OUT); put(b - 1, y, OUT)
    rect(x0, 185, x1 + 3, 186, OUT)
curtain(32, 62, False); curtain(196, 226, True)
rect(24, 28, 234, 32, DPURP); rect(24, 28, 234, 29, LILAC)
box(20, 26, 27, 34, YELLOW); box(231, 26, 238, 34, YELLOW)

# bed
box(30, 176, 46, 276, RED3); rect(32, 178, 44, 180, RED2)
box(46, 214, 256, 256, CREAM)
box(52, 200, 94, 222, CREAM); rect(53, 215, 93, 221, PAPER)
box(92, 212, 256, 252, BLUE)
rect(93, 213, 255, 216, BLUEL)
rect(93, 240, 255, 251, BLUE2)
for x in range(110, 250, 28): rect(x, 222, x + 2, 240, BLUE2)
rect(93, 216, 98, 251, BLUE3)
box(44, 254, 258, 264, RED3); rect(45, 255, 257, 256, RED2)
box(48, 262, 54, 276, RED3); box(246, 262, 252, 276, RED3)

# wall clock (9:00)
disc(268, 72, 15, OUT); disc(268, 72, 13, DPURP); disc(268, 72, 11, CREAM)
for a in range(12):
    put(268 + round(9 * math.cos(a * math.pi / 6)), 72 + round(9 * math.sin(a * math.pi / 6)), GRAY)
rect(267, 63, 269, 73, OUT); rect(261, 71, 269, 73, OUT); put(268, 72, RED)

# calendar
box(300, 52, 350, 112, CREAM)
box(300, 52, 350, 66, RED); rect(301, 53, 349, 55, C('#ff7a6a'))
for x in (312, 338): box(x - 2, 48, x + 2, 56, GRAY)
for r in range(5):
    for c in range(6):
        x, y = 305 + c * 7, 71 + r * 8
        rect(x, y, x + 4, y + 4, GRAY if (r, c) != (2, 2) else HOTPINK)
box(317, 84, 327, 94, CREAM, HOTPINK); rect(320, 87, 324, 91, HOTPINK)

# cabinet + TV with neon 333
blend(250, 116, 376, 212, CYAN, 0.14, lambda x, y: ((x - 313) / 62) ** 2 + ((y - 172) / 56) ** 2 <= 1)
box(260, 204, 368, 262, WOOD)
rect(261, 205, 367, 207, WOODL)
box(264, 210, 314, 256, WOOD2); box(314, 210, 364, 256, WOOD2)
rect(306, 230, 310, 234, YELLOW); rect(318, 230, 322, 234, YELLOW)
box(264, 258, 270, 268, WOOD3); box(358, 258, 364, 268, WOOD3)
for i in range(16):
    put(300 - i, 136 - i, OUT); put(326 + i, 136 - i, OUT)
disc(285, 121, 2, HOTPINK); disc(341, 121, 2, HOTPINK)
box(270, 136, 358, 206, NAVY)
rect(271, 137, 357, 139, PLUM)
box(276, 142, 346, 200, OUT)
rect(277, 143, 345, 199, C('#13263a'))
for y in range(144, 199, 3): rect(277, y, 345, y + 1, C('#0f1f30'))
# The day's quota digits are drawn on top of this screen at runtime.
blend(281, 152, 342, 192, CYAN, 0.12)
rect(350, 150, 354, 154, HOTPINK); rect(350, 160, 354, 164, GREEN)
rect(350, 170, 354, 172, GRAY); rect(350, 174, 354, 176, GRAY)

# rug
box(120, 288, 330, 330, RED)
box(126, 293, 324, 325, PINK, CREAM)
for x in range(134, 320, 12): rect(x, 300, x + 6, 318, C('#e89cb1'))
for x in range(122, 330, 4):
    put(x, 330, CREAM); put(x, 331, CREAM)

# door frame + mat (door sprite covers 462..550 x 132..260)
box(454, 124, 558, 261, PAPER)
rect(455, 125, 557, 127, CREAM); rect(555, 127, 557, 260, LILAC)
rect(462, 132, 550, 260, NAVY)
box(452, 264, 560, 278, GREEN)
for x in range(458, 556, 6): rect(x, 267, x + 3, 275, GREEN2)

# coat hook with bag
rect(568, 140, 604, 146, WOOD); rect(568, 140, 604, 141, WOODL); rect(568, 146, 604, 147, OUT)
box(576, 146, 580, 152, YELLOW)
box(570, 160, 600, 196, RED2); rect(571, 161, 599, 164, RED)
for i in range(14):
    put(578 - i // 2 + 0, 152 + i // 2 + 2, OUT); put(578 + i, 152 + i // 2 + 2, OUT)
box(580, 174, 590, 180, PEACH)

# potted plant
for cx, cy, r, c in [(612, 206, 9, GREEN2), (602, 216, 8, GREEN), (622, 218, 8, GREEN), (612, 196, 7, MINT), (608, 214, 6, MINT)]:
    disc(cx, cy, r + 1, OUT)
for cx, cy, r, c in [(612, 206, 9, GREEN2), (602, 216, 8, GREEN), (622, 218, 8, GREEN), (612, 196, 7, MINT), (608, 214, 6, MINT)]:
    disc(cx, cy, r, c)
box(598, 224, 626, 264, RED2); rect(599, 225, 625, 229, RED); rect(599, 252, 625, 263, RED3)

# soft shadows on floor
blend(30, 264, 262, 280, OUT, 0.25, lambda x, y: px[y][x] not in (RED3, OUT))
blend(258, 263, 370, 272, OUT, 0.25, lambda x, y: px[y][x] not in (WOOD3, OUT))
blend(596, 264, 628, 270, OUT, 0.25)

png.write(sys.argv[1], W, H, px)
