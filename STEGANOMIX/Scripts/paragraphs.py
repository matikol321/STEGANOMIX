import re

def text_to_bits(text):
    """Konwertuje tekst na ciąg bitów."""
    return ''.join(format(ord(c), '08b') for c in text)

def bits_to_text(bits):
    """Konwertuje ciąg bitów na tekst."""
    return ''.join(chr(int(bits[i:i+8], 2)) for i in range(0, len(bits), 8) if len(bits[i:i+8]) == 8)

def hide_message(cover_text, secret_message):
    """
    Ukrywa wiadomość w tekście przykrywkowym.
    Zwraca klucz stego.
    """
    secret_bits = text_to_bits(secret_message)
    bit_index = 0
    stego_key = ''

    words = re.findall(r'\b\w+\b', cover_text)

    for word in words:
        if bit_index >= len(secret_bits):
            break

        if word[0].lower() == word[-1].lower():
            continue

        bit = secret_bits[bit_index]

        if bit == '0':
            stego_key += word[0]
        else:
            stego_key += word[-1]
        bit_index += 1

    if bit_index < len(secret_bits):
        print(f"Ostrzeżenie: Nie udało się ukryć całej wiadomości. Ukryto {bit_index}/{len(secret_bits)} bitów.")

    return stego_key

def extract_message(cover_text, stego_key):
    """
    Wydobywa ukrytą wiadomość z tekstu przykrywkowego i klucza stego.
    """
    bits = ''
    words = re.findall(r'\b\w+\b', cover_text)
    key_index = 0

    for word in words:
        if key_index >= len(stego_key):
            break

        if word[0].lower() == word[-1].lower():
            continue

        s = word[0]
        e = word[-1]
        c = stego_key[key_index]

        if c == s:
            bits += '0'
        elif c == e:
            bits += '1'
        else:
            return ''  # Jeśli klucz nie pasuje, zwracamy pusty ciąg
        key_index += 1

    return bits_to_text(bits)

def main():
    import sys

    if len(sys.argv) < 3:
        print("Użycie:")
        print("  Ukrywanie: python script.py embed [cover.txt] ['message to hide']")
        print("  Wydobywanie: python script.py extract [cover.txt] [stego_key.txt]")
        sys.exit(1)

    mode = sys.argv[1]

    if mode == 'embed':
        if len(sys.argv) < 4:
            print("Użycie: python script.py embed [cover.txt] ['message to hide']")
            sys.exit(1)

        cover_file = sys.argv[2]
        secret_message = ' '.join(sys.argv[3:])  # Obsługuje wielowyrazowe wiadomości

        with open(cover_file, 'r', encoding='utf-8') as f:
            cover_text = f.read()

        stego_key = hide_message(cover_text, secret_message)

        with open('stego_key.txt', 'w', encoding='utf-8') as f:
            f.write(stego_key)

        print("Klucz stego został zapisany w pliku stego_key.txt.")

    elif mode == 'extract':
        if len(sys.argv) != 4:
            print("Użycie: python script.py extract [cover.txt] [stego_key.txt]")
            sys.exit(1)

        cover_file = sys.argv[2]
        stego_key_file = sys.argv[3]

        with open(cover_file, 'r', encoding='utf-8') as f:
            cover_text = f.read()

        with open(stego_key_file, 'r', encoding='utf-8') as f:
            stego_key = f.read()

        extracted_message = extract_message(cover_text, stego_key)
        print(extracted_message)

    else:
        print("Nieznany tryb działania. Użyj 'embed' lub 'extract'.")

if __name__ == "__main__":
    main()
