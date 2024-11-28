import re
from sys import argv

extension_char = '\u200B'  # Unicode U+200B (Zero Width Space)

# Zbiór liter do ukrywania bitów
target_letters = {'i', 'j', 'ż', 'ź', 'ó', 'ń', 'ś', 'ć'}

def text_to_bits(text):
    """Konwertuje tekst na ciąg bitów."""
    return ''.join(format(ord(c), '08b') for c in text)

def bits_to_text(bits):
    """Konwertuje ciąg bitów na tekst."""
    return ''.join(chr(int(bits[i:i+8], 2)) for i in range(0, len(bits), 8) if len(bits[i:i+8]) == 8)

def is_letter(char):
    """Sprawdza, czy znak jest literą."""
    return char.isalpha()

def can_hide_at(cover_text, index):
    """Sprawdza, czy można ukryć bit pod literą na pozycji index."""
    char = cover_text[index]
    if not is_letter(char) or char.lower() not in target_letters:
        return False
    prev_index = index - 1
    if prev_index < 0 or not is_letter(cover_text[prev_index]):
        return False
    next_index = index + 1
    if next_index >= len(cover_text) or not is_letter(cover_text[next_index]):
        return False
    return True

def can_hide_at_stego(stego_text, index):
    """Sprawdza, czy można ukryć bit pod literą na pozycji index w stegotekście."""
    char = stego_text[index]
    if char == extension_char or not is_letter(char) or char.lower() not in target_letters:
        return False
    prev_index = index - 1
    while prev_index >= 0 and stego_text[prev_index] == extension_char:
        prev_index -= 1
    if prev_index < 0 or not is_letter(stego_text[prev_index]):
        return False
    next_index = index + 1
    while next_index < len(stego_text) and stego_text[next_index] == extension_char:
        next_index += 1
    if next_index >= len(stego_text) or not is_letter(stego_text[next_index]):
        return False
    return True

def embed_message(cover_text, secret_message):
    """Ukrywa wiadomość w tekście bazowym."""
    bits = text_to_bits(secret_message)
    bit_index = 0
    stego_text = ''
    i = 0

    while i < len(cover_text):
        char = cover_text[i]
        stego_text += char

        if bit_index < len(bits) and can_hide_at(cover_text, i):
            if bits[bit_index] == '1':
                stego_text += extension_char
            bit_index += 1
        i += 1

    if bit_index < len(bits):
        print("Ostrzeżenie: Nie udało się ukryć całej wiadomości.")

    return stego_text

def extract_message(stego_text):
    """Wydobywa ukrytą wiadomość z tekstu steganograficznego."""
    bits = ''
    i = 0

    while i < len(stego_text):
        if can_hide_at_stego(stego_text, i):
            next_index = i + 1
            while next_index < len(stego_text) and stego_text[next_index] == extension_char:
                next_index += 1
            bits += '1' if next_index < len(stego_text) and stego_text[i + 1] == extension_char else '0'
        i += 1

    bits = bits[:len(bits) - (len(bits) % 8)]
    return bits_to_text(bits)

def main():
    if len(argv) < 3:
        print("Użycie:")
        print("  Ukrywanie: python script.py embed [cover.txt] ['message to hide'] [stego_output.txt]")
        print("  Wydobywanie: python script.py extract [stego_output.txt]")
        return

    mode = argv[1]

    if mode == 'embed':
        if len(argv) < 5:
            print("Użycie: python script.py embed [cover.txt] ['message to hide'] [stego_output.txt]")
            return

        cover_file = argv[2]
        secret_message = argv[3]
        output_file = argv[4]

        with open(cover_file, 'r', encoding='utf-8') as f:
            cover_text = f.read()

        stego_text = embed_message(cover_text, secret_message)

        with open(output_file, 'w', encoding='utf-8') as f:
            f.write(stego_text)

        print(f"Wiadomość została ukryta w pliku {output_file}.")

    elif mode == 'extract':
        if len(argv) < 3:  # Tryb extract wymaga tylko dwóch argumentów
            print("Użycie: python script.py extract [stego_output.txt]")
            return

        stego_file = argv[2]

        with open(stego_file, 'r', encoding='utf-8') as f:
            stego_text = f.read()

        extracted_message = extract_message(stego_text)
        print(extracted_message)

    else:
        print("Nieznany tryb działania. Użyj 'embed' lub 'extract'.")

if __name__ == "__main__":
    main()
