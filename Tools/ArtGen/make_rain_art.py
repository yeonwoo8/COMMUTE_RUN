import os, sys
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import png
from pixfont import C, CLEAR

ART = sys.argv[1]
def save(name, px): png.write(os.path.join(ART, name + '.png'), len(px[0]), len(px), px)

RED = C('#d64541'); RED_LIGHT = C('#f07b6f'); RED_SHADE = C('#9e2f3a'); LINE = C('#3b2a56'); HANDLE = C('#ffd257')

# umbrella: half-dome canopy with scalloped rim, shaft and a J handle
W, H = 26, 26
px = [[CLEAR] * W for _ in range(H)]
cx, r = 12.5, 12
for y in range(1, 14):
    for x in range(W):
        dx, dy = x - cx, 13 - y
        if dx * dx + dy * dy > r * r: continue
        if y == 13 and int(dx + 13) % 6 in (0, 5): continue
        px[y][x] = RED_LIGHT if (dx + dy * 0.6) < -4 else (RED_SHADE if int(dx + 13) % 6 == 3 else RED)
for y in range(13, 22):
    px[y][12] = px[y][13] = HANDLE
for x, y in ((14, 22), (15, 22), (16, 21), (16, 20), (11, 22), (12, 22), (13, 22)):
    px[y][x] = HANDLE
out = [row[:] for row in px]
for y in range(H):
    for x in range(W):
        if px[y][x][3]: continue
        if any(0 <= x + dx < W and 0 <= y + dy < H and px[y + dy][x + dx][3] for dx, dy in ((1, 0), (-1, 0), (0, 1), (0, -1))):
            out[y][x] = LINE
save('Umbrella', out)

# rain: tileable 64x64 of short slanted streaks
DROP = C('#bcd4ff')
tile = [[CLEAR] * 64 for _ in range(64)]
for i, (sx, sy) in enumerate([(3, 5), (19, 40), (33, 12), (47, 50), (58, 27), (11, 55), (27, 25), (41, 2), (53, 9), (7, 30), (37, 44), (61, 58)]):
    for k in range(6):
        tile[(sy + k) % 64][(sx - k // 2) % 64] = DROP
save('Rain', tile)
