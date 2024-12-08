import sys
import re
import bitstring


def text_to_binary(message):
    """Konwersja tekstu na ciąg bitów."""
    return ''.join(format(ord(char), '08b') for char in message)


def binary_to_text(binary_str):
    """Konwersja ciągu bitów z powrotem na tekst."""
    return ''.join(chr(int(binary_str[i:i + 8], 2)) for i in range(0, len(binary_str), 8))


def extract_pre_content(html_content):
    """Wyodrębnienie zawartości z tagów <pre>."""
    pre_pattern = r'<pre>(.*?)</pre>'
    matches = re.findall(pre_pattern, html_content, re.DOTALL)
    if matches:
        return '\n'.join(matches)
    else:
        return ''


def embed_message(cover_html, message):
    """Osadzanie wiadomości w pliku HTML."""
    binary_message = text_to_binary(message) + '00000000'  # Dodanie terminatora
    binary_stream = bitstring.BitStream(bin=binary_message)

    with open(cover_html, 'r', encoding='utf-8') as f:
        html_content = f.read()

    pre_content = extract_pre_content(html_content)
    lines = pre_content.split('\n')

    output_lines = []
    bit_index = 0

    for line in lines:
        if bit_index < len(binary_message):
            # Wyciągnięcie 2 bitów
            block = binary_stream.read('bin:2')

            # Modyfikacja linii w zależności od bloków bitowych
            if block == '00':
                # Zmniejsz nieznacznie rozmiar czcionki (99%)
                modified_line = f'<span style="font-size:99%;">{line}</span>'
            elif block == '11':
                # Zwiększ nieznacznie rozmiar czcionki (101%)
                modified_line = f'<span style="font-size:101%;">{line}</span>'
            elif block == '01':
                # Dodaj podwójną spację na końcu linii
                modified_line = line.rstrip() + '  '
            elif block == '10':
                # Dodaj dodatkową spację przed znakiem specjalnym lub dodaj kropkę na końcu
                special_char_match = re.search(r'([^\w\s])', line)
                if special_char_match:
                    special_char = special_char_match.group(1)
                    modified_line = line.replace(special_char, ' ' + special_char, 1)
                else:
                    modified_line = line + '.'

            output_lines.append(modified_line)
            bit_index += 2
        else:
            output_lines.append(line)

    updated_html = html_content.replace(pre_content, '\n'.join(output_lines))
    return updated_html


def extract_message(stego_html):
    """Wyodrębnianie wiadomości z pliku HTML."""
    with open(stego_html, 'r', encoding='utf-8') as f:
        html_content = f.read()

    pre_content = extract_pre_content(html_content)
    lines = pre_content.split('\n')
    extracted_bits = []

    for line in lines:
        # Analiza rozmiaru czcionki
        font_size_match = re.search(r'font-size:(\d+)%', line)
        if font_size_match:
            size = int(font_size_match.group(1))
            if size == 99:
                extracted_bits.append('00')
            elif size == 101:
                extracted_bits.append('11')
            continue

        # Analiza spacji
        if line.endswith('  '):
            extracted_bits.append('01')
            continue

        # Analiza znaków specjalnych i kropek
        special_char_match = re.search(r' ([^\w\s])', line)
        if special_char_match:
            extracted_bits.append('10')
            continue

        if line.endswith('.'):
            extracted_bits.append('10')
            continue

    # Konwersja bitów na tekst
    binary_message = ''.join(extracted_bits)
    # Usuwanie dopełnienia
    while len(binary_message) % 8 != 0:
        binary_message = binary_message[:-1]

    try:
        message = binary_to_text(binary_message)
        return message.rstrip('\x00')
    except:
        return "Nie udało się zdekodować wiadomości."


def main():
    if len(sys.argv) < 3:
        print("""Użycie:
  Ukrywanie: python mixed.py embed [cover.html] 'tajna wiadomość' [stego.html]
  Wydobywanie: python mixed.py extract [stego.html]""")
        sys.exit(1)

    mode = sys.argv[1]

    if mode == 'embed':
        if len(sys.argv) != 5:
            print("Nieprawidłowa liczba argumentów dla trybu embed")
            sys.exit(1)

        cover_html = sys.argv[2]
        message = sys.argv[3]
        stego_html = sys.argv[4]

        stego_content = embed_message(cover_html, message)

        with open(stego_html, 'w', encoding='utf-8') as f:
            f.write(stego_content)

        print(f"Wiadomość została osadzona w pliku {stego_html}")

    elif mode == 'extract':
        if len(sys.argv) != 3:
            print("Nieprawidłowa liczba argumentów dla trybu extract")
            sys.exit(1)

        stego_html = sys.argv[2]
        message = extract_message(stego_html)

        print(message)

    else:
        print("Nieznany tryb. Użyj 'embed' lub 'extract'.")
        sys.exit(1)


if __name__ == '__main__':
    main()