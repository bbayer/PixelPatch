from PIL import Image
import sys

for ndx,img in enumerate(sys.argv[1:]):

    print "//%s"%img
    print "//%d"%ndx
    im = Image.open(img)
    rgb_im= im.convert('RGBA')
    print "levelData.Add(new Color32[]{"
    #print "new Color32[]{"
    for i in range(im.size[0]):
        for j in range(im.size[1]):
            px = rgb_im.getpixel((i,j))
            print "new Color32(%d,%d,%d,%d)," %px,

    print "});"
    print
