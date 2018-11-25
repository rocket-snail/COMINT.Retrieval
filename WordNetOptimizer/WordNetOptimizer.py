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

inputdir = "../Workplace/Google/Text"
outputdir = "../Workplace/Google/Syns"


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
    print('Argument List:', str(sys.argv))
    print('Loading data', file=sys.stderr)
    if len(sys.argv) >= 3:
        inputdir = sys.argv[1]
        outputdir = sys.argv[2]
    print('Input Directory: ', inputdir)
    print('Output Directory: ', outputdir)
    
    filesToRead = [f for f in listdir(inputdir) if isfile(join(inputdir, f))]
    read_files(filesToRead, inputdir, outputdir)
    print('Finished google files')

