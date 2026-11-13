import os, sys
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import png
from pixfont import C, neon_sign

px = neon_sign([('COMMUTE', C('#ffd257')), ('RUN', C('#49e8ff'))], scale=4, frame=C('#ff4fa3'))
png.write(sys.argv[1], len(px[0]), len(px), px)
print(len(px[0]), len(px))
