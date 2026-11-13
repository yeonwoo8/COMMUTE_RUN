import os, sys
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import png
from pixfont import text_cells, text_width

OUT_DIR = sys.argv[1]
def C(h): return (int(h[1:3], 16), int(h[3:5], 16), int(h[5:7], 16), 255)
CLEAR = (0, 0, 0, 0)
OUT = C('#1c1230'); CREAM = C('#fbf1ea'); YELLOW = C('#ffd257'); LIGHT = C('#fff4e2')
RED = C('#d8574a'); RED_HI = C('#ff7a6a'); RED_HOVER = C('#e8695a'); RED_DARK = C('#824941')
PLUM = C('#3b2a56'); DUST = C('#fff8f2'); DUST2 = C('#f0dfd6'); DUST3 = C('#a8919a')


def canvas(w, h): return [[CLEAR] * w for _ in range(h)]

def rect(px, x0, y0, x1, y1, c):
    for y in range(y0, y1):
        for x in range(x0, x1):
            px[y][x] = c


# START button: 112x44, a 4px bevel key that sinks when pressed
BW, BH, BEVEL = 112, 44, 4

def button(body, ring, sink):
    px = canvas(BW, BH)
    top = sink
    face_bottom = BH - BEVEL + sink - (1 if sink else 0)
    rect(px, 1, top + 1, BW - 1, BH - 1, OUT)
    rect(px, 0, top + 2, BW, BH - 2, OUT)
    rect(px, 2, top + 2, BW - 2, BH - 2, RED_DARK)
    rect(px, 2, top + 2, BW - 2, face_bottom, body)
    rect(px, 3, top + 3, BW - 3, top + 5, RED_HI if not ring else LIGHT)
    rect(px, 2, face_bottom - 1, BW - 2, face_bottom, RED_DARK)
    if ring:
        rect(px, 2, top + 2, BW - 2, top + 3, YELLOW); rect(px, 2, face_bottom - 2, BW - 2, face_bottom - 1, YELLOW)
        rect(px, 2, top + 2, 3, face_bottom - 1, YELLOW); rect(px, BW - 3, top + 2, BW - 2, face_bottom - 1, YELLOW)
    ox = (BW - text_width('START', 3, 3)) // 2
    oy = top + (face_bottom - top - 21) // 2 + 1
    for x, y, gy in text_cells('START', 3, 3):
        px[oy + y + 2][ox + x + 1] = RED_DARK
    for x, y, gy in text_cells('START', 3, 3):
        px[oy + y][ox + x] = CREAM if gy < 5 else C('#f2d6c8')
    return px

png.write(os.path.join(OUT_DIR, 'StartButton.png'), BW, BH, button(RED, False, 0))
png.write(os.path.join(OUT_DIR, 'StartButtonHover.png'), BW, BH, button(RED_HOVER, True, 0))
png.write(os.path.join(OUT_DIR, 'StartButtonPressed.png'), BW, BH, button(RED, True, 3))

# Dust puff, 3 frames of 12x10: dense -> wide -> breaking up
DUST_FRAMES = [
    ['............',
     '............',
     '............',
     '............',
     '....aab.....',
     '...aaaab....',
     '..baaabbc...',
     '...bbbcc....',
     '............',
     '............'],
    ['............',
     '............',
     '.....aa.....',
     '...aaaaab...',
     '..aaaaaabb..',
     '.baaaabbbc..',
     '.bbbbbbcc...',
     '..ccc.cc....',
     '............',
     '............'],
    ['............',
     '....a....a..',
     '..aab...ab..',
     '.abbc..abbc.',
     '..cc....cc..',
     '......a.....',
     '.....bbc....',
     '......c.....',
     '............',
     '............'],
]
COLORS = {'a': DUST, 'b': DUST2, 'c': DUST3}
for i, rows in enumerate(DUST_FRAMES):
    px = [[COLORS.get(ch, CLEAR) for ch in row] for row in rows]
    png.write(os.path.join(OUT_DIR, 'Dust_%d.png' % i), 12, 10, px)

# Pickup sparkle, 7x7 four-point star
STAR =['...o...', '..oyo..', '.oylyo.', 'oylllyo', '.oylyo.', '..oyo..', '...o...']
COLORS = {'o': OUT, 'y': YELLOW, 'l': LIGHT}
px = [[COLORS.get(ch, CLEAR) for ch in row] for row in STAR]
png.write(os.path.join(OUT_DIR, 'Spark.png'), 7, 7, px)
