%%Parameters:
original_audio_dir = 'D:\Data\COMINT.Retrieval\Windows\Speech';
noisy_audio_dir = 'D:\Data\COMINT.Retrieval\Windows\Noise\0.5';

block_length_noise = 50000;
noise_factor = 0.5;

%Load directory and add files to string array
files = dir(original_audio_dir);
for i = 1:length(files)
    file = files(i);
    path = strcat(file.folder, '\', file.name);
    if ~endsWith(file.name, '.wav')
        continue
    end
    % Read in audio file
    fprintf('Transform file: %s\n', path);
    [sound, freq] = audioread(path);
    
    %Generate noise matrix
    matrix = zeros(1, length(sound));
    n = 1;
    p = 1;
    while n < length(sound)
        x = int32(rand * block_length_noise);
        temp = (-1)^p;
        for i = 1 : x
            if n < length(sound) && temp == 1
                matrix(n) = temp;
            end
            n = n + 1;
        end
        p = p +1;
    end
    matrix = matrix.';
    
    %Multiply nosie matrix with original audio file
    noise = (0.1 * randn(length(sound), 1)) .* matrix;
    no_noise = ones(length(sound), 1) - (matrix * noise_factor);
    
    noise_sound = (sound .* no_noise) + noise;    
    %Write noise audiofile
    file = strcat(noisy_audio_dir, '\', 'Noise_', num2str(noise_factor), '_', num2str(block_length_noise), '_',  file.name);
    audiowrite(file, noise_sound, freq);
    fprintf('Finhsed with output: %s\n', file);
end

