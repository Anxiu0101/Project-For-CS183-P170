import qrcode
# example data
data = "https://github.com/Anxiu0101/Project-For-CS183-P170"
# output file name
filename = "site.png"
# generate qr code
img = qrcode.make(data)
# save img to a file
img.save(filename)