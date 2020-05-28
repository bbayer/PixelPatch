from PIL import Image
import sys


def main():
    if len(sys.argv)!=3:
        print("usage: slice.py filename prefix")
        sys.exit(1)
    fname = sys.argv[1]
    prefix = sys.argv[2]
    
    im = Image.open(fname)
    rgb_im= im.convert('RGBA')
    width = im.size[0]
    height = im.size[1]

    if width % 16 !=0 or height % 16 !=0:
        print("Image size is not 16")
        sys.exit(1)
    count=0
    for dx in range(width/16):
        for dy in range(height/16):
            tmp_img = im.crop((dx*16,dy*16,dx*16+16,dy*16+16))
            fname = "%s_%03d.gif" %(prefix,count)
            tmp_img.save(fname)
            print("%s saved.." % fname)
            count=count+1 


main()
