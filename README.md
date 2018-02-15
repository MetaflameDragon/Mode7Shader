# Mode7Shader

The Mode 7 Shader has 8 exposed values and 1 input texture:

H: Horizontal offset
V: Vertical offset
X0: Pivot X position
Y0: Pivot Y position

A, B, C, D: Values which define the transformation like a parallelogram.
  A and D represent the width and height, C and D are used for the shearing effect.

Video explanation of SNES Background Mode 7 (by dotsarecool/Retro Game Mechanics Explained):
https://www.youtube.com/watch?v=3FVN_Ze7bzw&t=784s
This video also acted as a source for all the math involved in this.


The Mode7Controller.cs script provides a list of Mode7Config structs (a Mode7Config struct stores all of the 8 exposed properties).
Mode7ControllerEditor.cs can then be used (as a CustomEditor) to modify/add/remove these values and linearly interpolate towards a selected value along a curve (which also modifies the material values).
