# -*- coding: utf-8 -*-
"""
Created on Fri Aug  6 12:46:56 2021

@author: Willy
"""

# Some standard imports
import io
import numpy as np

from torch import nn
import torch.utils.model_zoo as model_zoo
import torch.onnx
from models.xception import xception

import argparse

parser = argparse.ArgumentParser()
parser.add_argument('-model', type=str, required=True, help='pytorch model path')
args = parser.parse_args()
    
batch_size = 1
# Input to the model
x = torch.randn(batch_size, 3, 32, 32, requires_grad=True)

# Initialize model with the pretrained weights
torch_model = xception()
torch_model.load_state_dict(torch.load(args.model))

# set the model to inference mode
torch_model.eval()

torch_out = torch_model(x)#the output after of the model, which we will use to verify that the model we exported computes the same values when run in ONNX Runtime.

# Export the model
torch.onnx.export(torch_model,               # model being run
                  x,                         # model input (or a tuple for multiple inputs)
                  "xception_humean_detect.onnx",   # where to save the model (can be a file or file-like object)
                  export_params=True,        # store the trained parameter weights inside the model file
                  opset_version=10,          # the ONNX version to export the model to
                  do_constant_folding=True,  # whether to execute constant folding for optimization
                  input_names = ['input'],   # the model's input names
                  output_names = ['output'], # the model's output names
                  dynamic_axes={'input' : {0 : 'batch_size'},    # variable length axes
                                'output' : {0 : 'batch_size'}})