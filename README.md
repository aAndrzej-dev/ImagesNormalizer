# Images Normalizer
A command line utility to convert multiple images.

## Features
- Standarizing images size with size auto detecting
- Adding margin
- Adding border

## Usgae

ImagesNormalizer.exe source_dir target_dir [-s auto] [-s width height] [-m all] [-m left top right bottom] [-b color all] [-b color left top right bottom]

Options:
- -s auto&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;Automaticly detect the normalized size for all target images (includes margin)
- -s width height &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp; Sets the normalized size for target images. If a source image is larger, the image is skipped
- -m all &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp; Sets the margin for all sides 
- -m left top right bottom &emsp;&emsp;&emsp; Sets the margin for every side individually
- -b color all &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp; Sets the border for all sides; color in HTML format.
- -m color left top right bottom&emsp;Sets the margin for every side individually; color in HTML format.
