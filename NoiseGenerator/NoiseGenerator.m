%%Parameters:
original_audio_dir = 'D:\GitHub\COMINT.Retrieval\COMINT.Retrieval\Documents\audio';
noisy_audio_dir = 'D:\GitHub\COMINT.Retrieval\NoiseGenerator\NoisedAudioFiles2';
block_length_noise = 50000;


%Load directory and add files to string array
filemat = dir(original_audio_dir);
files = strings (1,length(filemat)-2)';
for i = 3:length(filemat)
    files(i-2) = strcat(filemat(i,1).folder,"\",filemat(i,1).name);
end

%Create noised wav-file for each audiofile
for k = 1 : length(files)
    % Read in audio file
    curfile = char(files(k));
    [perfectSound, freq] = audioread(curfile);
    
    %Generate noise matrix
    matrix = zeros(1, length(perfectSound));
    n = 1;
    p = 1;
    while n < length(perfectSound)
        x = int32(rand * block_length_noise);
        temp = (-1)^p;
        for i = 1 : x
            if n < length(perfectSound) && temp == 1
                matrix(n) = temp;
            end
            n = n + 1;
        end
        p = p +1;
    end
    matrix = matrix.';
    
    %Multiply nosie matrix with original audio file
    noisySound = perfectSound .* ~matrix +  (0.1 * randn(length(perfectSound), 1)) .* matrix;
    %Write noise audiofile
    audiowrite(strcat(noisy_audio_dir, '\noised_',filemat(k+2,1).name),noisySound, freq);
    fprintf(strcat('finished  ', int2str(k),'\n'));
end

