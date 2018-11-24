"""
Filreader enriching files with synonyms out of wordnet
"""

import sys
import nltk
from nltk.corpus import wordnet
from os import listdir
from os.path import join, isfile


__author__ = "kaufmann-a@hotmail.ch"

nltk.download('wordnet')

inputdirGoogle = "../Workplace/Google/Text"
outputdirGoogle = "../Workplace/Google/Syns"
inputdirWindows = "../Workplace/Windows/Text/1"
outputdirWindows = "../Workplace/Windows/Syns/1"


def read_files(filestoread, inputdirectory, outputdirectory):

    for file in filestoread:
        f = open(inputdirectory + "/" + file, "r")
        words = []
        if f.mode == 'r':
            words = f.read().lower().split()
            words.extend(add_synonyms_to_wordarray(words))
        newfile = open(outputdirectory + "/" + file, "w")
        newfile.write(' '.join(words))
        newfile.close()

def add_synonyms_to_wordarray(wordarray):
    tempSynonymsArray = []
    for word in wordarray:
        syns = wordnet.synsets(word)
        if len(syns) > 0:
            for syn in syns:
                if syn.lemmas()[0].name() != word:
                    tempSynonymsArray.append(syn.lemmas()[0].name())
                    break
    return tempSynonymsArray

if __name__ == '__main__':
    print('Loading data', file=sys.stderr)
    "Process files Google"
    filesToRead = [f for f in listdir(inputdirGoogle) if isfile(join(inputdirGoogle, f))]
    read_files(filesToRead, inputdirGoogle, outputdirGoogle)
    print('Finished google files')
    "Process files WIndows"
    filesToRead = [f for f in listdir(inputdirWindows) if isfile(join(inputdirWindows, f))]
    read_files(filesToRead, inputdirWindows, outputdirWindows)
    print('Finished windows files')
