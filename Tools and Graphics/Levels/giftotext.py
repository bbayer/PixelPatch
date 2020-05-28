from PIL import Image
import sys

for ndx,img in enumerate(sys.argv[1:]):
    fl = open("%d.bytes"%ndx,"wb")
    print "//%s"%img
    im = Image.open(img)
    rgb_im= im.convert('RGBA')
    for i in range(im.size[0]):
        for j in range(im.size[1]):
            px = rgb_im.getpixel((i,j))
            fl.write(bytearray(px))
    fl.close()

