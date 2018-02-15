# Mode7Shader

The Mode 7 Shader has 8 exposed properties and 1 input texture:

H: Horizontal offset
V: Vertical offset
X0: Pivot X position
Y0: Pivot Y position

A, B, C, D: Values which define the transformation like a parallelogram.
  A and D represent the width and height, C and D are used for the shearing effect.

Video explanation of SNES Background Mode 7 (by dotsarecool/Retro Game Mechanics Explained):
https://www.youtube.com/watch?v=3FVN_Ze7bzw&t=784s
This video also acted as a source for all the math involved in this.
