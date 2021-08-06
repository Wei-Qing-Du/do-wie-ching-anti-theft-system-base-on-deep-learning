# -*- coding: utf-8 -*-
"""
Created on Fri Aug  6 12:46:56 2021

@author: Willy
"""

# Some standard imports
import onnx
import onnxruntime
import torchvision.transforms as transforms
from torch.autograd import Variable
from PIL import Image
import argparse

coarse_label = [
'apple', # id 0
'aquarium_fish',
'baby',
'bear',
'beaver',
'bed',
'bee',
'beetle',
'bicycle',
'bottle',
'bowl',
'boy',
'bridge',
'bus',
'butterfly',
'camel',
'can',
'castle',
'caterpillar',
'cattle',
'chair',
'chimpanzee',
'clock',
'cloud',
'cockroach',
'couch',
'crab',
'crocodile',
'cup',
'dinosaur',
'dolphin',
'elephant',
'flatfish',
'forest',
'fox',
'girl',
'hamster',
'house',
'kangaroo',
'computer_keyboard',
'lamp',
'lawn_mower',
'leopard',
'lion',
'lizard',
'lobster',
'man',
'maple_tree',
'motorcycle',
'mountain',
'mouse',
'mushroom',
'oak_tree',
'orange',
'orchid',
'otter',
'palm_tree',
'pear',
'pickup_truck',
'pine_tree',
'plain',
'plate',
'poppy',
'porcupine',
'possum',
'rabbit',
'raccoon',
'ray',
'road',
'rocket',
'rose',
'sea',
'seal',
'shark',
'shrew',
'skunk',
'skyscraper',
'snail',
'snake',
'spider',
'squirrel',
'streetcar',
'sunflower',
'sweet_pepper',
'table',
'tank',
'telephone',
'television',
'tiger',
'tractor',
'train',
'trout',
'tulip',
'turtle',
'wardrobe',
'whale',
'willow_tree',
'wolf',
'woman',
'worm',
]

parser = argparse.ArgumentParser()
parser.add_argument('-image', type=str, required=True, help='test image path')
parser.add_argument('-onnx', type=str, required=True, help='onnx model path')
args = parser.parse_args()

ort_session = onnxruntime.InferenceSession(args.onnx)

loader = transforms.Compose([transforms.Resize((32, 32)), transforms.RandomCrop(32),transforms.ToTensor()])
image = Image.open(args.image)
image = loader(image).float()
image = Variable(image, requires_grad=True)
image = image.unsqueeze(0)  #this is for VGG, may not be needed for ResNet

def to_numpy(tensor):
    return tensor.detach().cpu().numpy() if tensor.requires_grad else tensor.cpu().numpy()

ort_inputs = {ort_session.get_inputs()[0].name: to_numpy(image)}
ort_outs = ort_session.run(None, ort_inputs)[0]
print("onnx prediction", coarse_label[ort_outs.argmax(axis=1)[0]])