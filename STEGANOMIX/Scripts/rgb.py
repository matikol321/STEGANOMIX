import argparse
from PIL import Image
import numpy as np
import unicodedata

def remove_polish_characters(text):
    # Słownik zamiany polskich znaków na ich odpowiedniki bez akcentów
    replacements = {
        "ą": "a", "ć": "c", "ę": "e", "ł": "l", "ń": "n", "ó": "o", "ś": "s", "ź": "z", "ż": "z",
        "Ą": "A", "Ć": "C", "Ę": "E", "Ł": "L", "Ń": "N", "Ó": "O", "Ś": "S", "Ź": "Z", "Ż": "Z"
    }
    
    # Zamiana polskich znaków na ich odpowiedniki bez akcentów
    for polish_char, ascii_char in replacements.items():
        text = text.replace(polish_char, ascii_char)
    
    return text

def embed_secret(image_path, secret_data, output_path, is_file=False):
 # 1. Jeśli `is_file` jest True, odczytaj sekretną wiadomość z pliku
    if is_file:
        with open(secret_data, 'r', encoding='utf-8') as file:
            secret_data = file.read()

    # Dodaj znacznik końca do sekretnej wiadomości
    secret_data += "#END#"

    # 2. Usuń polskie znaki z wiadomości
    secret_data = remove_polish_characters(secret_data)
    
    # 3. Konwertuj sekretną wiadomość na ciąg binarny
    secret_bin = ''.join(format(ord(char), '08b') for char in secret_data)
    
    # 4. Oblicz długość sekretnej wiadomości w bitach
    length = len(secret_bin)
    
    # 5. Otwórz obraz i zamień go na tablicę
    img = Image.open(image_path)
    img_array = np.array(img)

    # 6. Wyciągnij warstwę B i zapamiętaj jej wymiary
    image_b = img_array[..., 2]
    original_shape = image_b.shape
    
    # 7. Spłaszcz warstwę B z 2D do 1D
    image_b_flat = image_b.flatten()

    # 8. Sprawdź, czy wiadomość nie jest za długa dla danego obrazu 
    if len(secret_data) * 8 > len(image_b_flat):
        print("Tekst do ukrycia jest za długi dla wybranego obrazu. Spróbuj z innym obrazem")
        exit()
    
    # 9. Zamień LSB pixela na wartość bitu ukrywanej wiadomości
    for index in range(length):
        b_pixel = bin(image_b_flat[index])
        b_pixel = b_pixel[:-1] + secret_bin[index]
        b_pixel = int(b_pixel, 2)
        image_b_flat[index] = b_pixel
    
    # 10. Przywróć zmodyfikowaną płaszczyznę B do pierwotnego kształtu
    image_b_modified = image_b_flat.reshape(original_shape)
    
    # 11. Podmień zmodyfikowaną płaszczyznę B w obrazie
    img_array[..., 2] = image_b_modified

    if output_path.lower().endswith(".jpg") or output_path.lower().endswith(".jpeg"):
        output_path = output_path.replace(".jpg", ".png")
        output_path = output_path.replace(".jpeg", ".png")
        print("Ostrzeżenie: Zapisywanie w formacie JPEG może spowodować utratę danych z powodu kompresji stratnej. Zmieniono format na PNG dla bezpiecznego zapisu.")
    
    # 12. Zapisz obraz z ukrytą wiadomością
    Image.fromarray(img_array).save(output_path)
    print(f"Secret data embedded successfully and saved to {output_path}")

def extract_secret(stego_image_path):
    # 1. Otwórz obraz i zamień go na tabelę
    img = Image.open(stego_image_path)
    img_array = np.array(img)

    # 2. Wyodrębnij kanału Blue z obrazu
    image_b = img_array[..., 2]
    
    # 3. Zamień tablicę 2D na 1D 
    image_b_flat = image_b.flatten()

    # 4. Zamiana wartości na 8-bitową reprezentację binarną
    image_b_bin = [format(int(value), '08b') for value in image_b_flat]

    # 5. Wyciągnij LSB z pixela i szukaj wzorca końca wiadomości  
    pattern = "0010001101000101010011100100010000100011" # "#END#"

    wiadomosc = ""
    for bin_value in image_b_bin:
        wiadomosc += bin_value[-1]  # Dodajemy tylko najmniej znaczący bit

        if pattern in wiadomosc:  # Sprawdzamy, czy wzorzec pojawił się w wiadomości
            # print("Wzorzec znaleziony! Zatrzymujemy pętlę.")
            break

    # 6. Usuń z wiadomości wzorzec końca wiadomości
    wiadomosc = wiadomosc.replace("0010001101000101010011100100010000100011", "")

    # 7. Konweruj binarny ciąg na tekst
    secret_text = ""
    for i in range(0, len(wiadomosc), 8): # Przejdź przez ciąg binarny po 8 bitów na raz   
        byte = wiadomosc[i:i+8] # Pobierz 8-bitowy fragment
        secret_text += chr(int(byte, 2)) # Zamień na liczbę całkowitą (int) w systemie binarnym, a potem na znak ASCII
    
    # 8. Wypisz wydobytą wiadomość
    print(secret_text)
    return secret_text

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Steganography tool for embedding and extracting secret data in images.")
    subparsers = parser.add_subparsers(dest="command", help="Choose 'embed' to hide data or 'extract' to retrieve data")
    
    # Subparser dla wstawiania sekretnych danych
    embed_parser = subparsers.add_parser("embed", help="Embed secret data into an image")
    embed_parser.add_argument("image_path", help="Path to the cover image file")
    embed_parser.add_argument("secret_data", help="Secret data to embed in the image (text or file path)")
    embed_parser.add_argument("output_path", help="Path to save the output stego image. Don't use .jpeg or .jpg because of lossy compression")
    embed_parser.add_argument("--file", action="store_true", help="Indicates that secret_data is a file path")

    # Subparser dla wydobywania sekretnych danych
    extract_parser = subparsers.add_parser("extract", help="Extract secret data from a stego image")
    extract_parser.add_argument("stego_image_path", help="Path to the stego image file")

    args = parser.parse_args()
    
    if args.command == "embed":
        embed_secret(args.image_path, args.secret_data, args.output_path, is_file=args.file)
    elif args.command == "extract":
        extract_secret(args.stego_image_path)
    else:
        parser.print_help()