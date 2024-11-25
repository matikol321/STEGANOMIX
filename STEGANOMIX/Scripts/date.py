import argparse
import random

ASCII_List = [
    [1], # 0
    [2, 3], # 1
    [4, 5, 6], # 2
    [7, 8, 9, 10], # 3
    [11, 12, 13, 14, 15], # 4
    [16, 17, 18, 19, 20, 21], # 5
    [22, 23, 24, 25, 26, 27, 28], # 6
    [29, 30, 31, 32, 33, 34, 35, 36], # 7
    [37, 38, 39, 40, 41, 42, 43, 44, 45], # 8
    [46, 47, 48, 49, 50, 51, 52, 53, 54, 55], # 9
    [56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66], # 10
    [67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78], # 11
    [79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91], # 12
    [92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105], # 13
    [106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120], # 14
    [121, 122, 123, 124, 125, 126, 127] # 15
]

def remove_polish_characters(text):
    # 1. Tworzenie słownika zamiany polskich znaków na ich odpowiedniki bez akcentów
    replacements = {
        "ą": "a", "ć": "c", "ę": "e", "ł": "l", "ń": "n", "ó": "o", "ś": "s", "ź": "z", "ż": "z",
        "Ą": "A", "Ć": "C", "Ę": "E", "Ł": "L", "Ń": "N", "Ó": "O", "Ś": "S", "Ź": "Z", "Ż": "Z"
    }
    
    # 2. Zamiana polskich znaków na ich odpowiedniki bez akcentów
    for polish_char, ascii_char in replacements.items():
        text = text.replace(polish_char, ascii_char)
    
    return text


def find_position(number, groups):
    # 1. Przeszukiwanie grup, aby znaleźć pozycję liczby
    for M, group in enumerate(groups):
        if number in group:
            N = group.index(number)
            return M, N
    return None  # 2. Zwraca None, jeśli liczba nie została znaleziona

# Funkcja do kodowania wiadomości
def encode_message(message):
    # 1. Usuwanie polskich znaków z wiadomości
    message = remove_polish_characters(message)
    encoded_pairs = []
    # 2. Kodowanie każdej litery wiadomości na pary (M, N)
    for char in message:
        ascii_value = ord(char)
        
        # 3. Znajdowanie odpowiednich grup (M, N) na podstawie formuły
        group, position = find_position(ascii_value, ASCII_List)
        
        # 4. Dodanie pary (M, N) do listy
        encoded_pairs.append((group, position + 1))
    return encoded_pairs

# Funkcja do dekodowania wiadomości
def decode_message(encoded_pairs):
    # 1. Dekodowanie wiadomości z par (M, N)
    decoded_message = ""
    for group, position in encoded_pairs:
        ascii_value = (group * (group + 1) // 2) + position
        decoded_message += chr(ascii_value)
    return decoded_message

# Funkcja do znalezienia grupy (M) i pozycji (N) dla danego ASCII
def find_group_position(ascii_value):
    # 1. Określenie grupy na podstawie wartości ASCII
    group = 1
    while (group * (group + 1)) // 2 < ascii_value:
        group += 1
    # 2. Obliczenie pozycji w grupie
    position = ascii_value - ((group * (group - 1)) // 2)
    return group, position

# Funkcja do tworzenia zakodowanego tekstu (ukrytego w dacie)
def hide_message_in_text(text, message, output_path, template=False):
    # 1. Sprawdzenie, czy używamy szablonu
    if template:
        try:
            with open(text, 'r', encoding='utf-8') as file:
                hidden_text = file.read()
        except:
            print("Warning: --template option set, but no template provided.\n")
            hidden_text = text
            # 2. Sprawdzenie, czy wiadomość zmieści się w dostępnych liniach tekstu
        available_places = hidden_text.count("DD/MM")
        if len(message) > available_places:
            print(f"The message is too long ({len(message)}) to hide in the given text (available lines: {available_places}).\n")
            exit()
    else:
        hidden_text = text
        
    # 3. Kodowanie wiadomości
    encoded_pairs = encode_message(message)
    idx = 0
    # 4. Ukrywanie zakodowanej wiadomości w dacie w tekście
    for group, position in encoded_pairs:
        # 5. Szukamy daty w formacie DD/MM w tekście
        if idx < len(hidden_text):
            start_idx = hidden_text.find("DD/MM", idx)
            if start_idx == -1:
                break
            hidden_text = hidden_text[:start_idx] + f"{group}/{position}" + hidden_text[start_idx + 5:]
            idx = start_idx + 6  # 6. Przechodzimy do następnej daty w tekście

    # 7. Wstawienie znacznika końca ("1/1") tam, gdzie pozostały "DD/MM"
    if "DD/MM" in hidden_text:
        hidden_text = hidden_text.replace("DD/MM", "1/1", 1)  # Zastąpienie pierwszego napotkanego DD/MM specjalnym znakiem końca
    
    # 8. Zamiana pozostałych "DD/MM" na losowe daty
    while "DD/MM" in hidden_text:
        random_day = random.randint(1, 31)
        random_month = random.randint(1, 12)
        hidden_text = hidden_text.replace("DD/MM", f"{random_day:02}/{random_month:02}", 1)

    # 9. Zapisanie zakodowanego tekstu do pliku
    with open(output_path, 'w', encoding='utf-8') as file:
        file.write(hidden_text)
    return hidden_text

# Funkcja do wydobywania zakodowanej wiadomości z ukrytego tekstu
def extract_message_from_text(text, is_file=False):
    # 1. Sprawdzenie, czy tekst jest w pliku
    if is_file:
        try:
            with open(text, 'r', encoding='utf-8') as file:
                text = file.read()
        except:
            print("Warning: --file option set, but no file provided.\n")
            exit()
    # 2. Wyszukiwanie par (M, N) w tekście
    import re
    pairs = re.findall(r"(\d{1,2})/(\d{1,2})", text) # Wyszukuje tekst w formacie DD/MM

    decoded_pairs = []
    for m, n in pairs:
        # 3. Sprawdzanie, czy napotkaliśmy specjalny znak końca (1/1)
        if m == '1' and n == '1':
            # Jeśli znajdziemy "1/1", zakończ dekodowanie
            break
        decoded_pairs.append((int(m), int(n)))
    
    # 4. Dekodowanie wiadomości z par
    return decode_message(decoded_pairs)

if __name__ == "__main__":
    # 1. Tworzenie parsera argumentów
    parser = argparse.ArgumentParser(description="Steganography tool for hiding and extracting secret messages in text using model number system.")
    subparsers = parser.add_subparsers(dest="command", help="Choose 'hide' to hide data or 'extract' to retrieve data")

    # 2. Subparser do ukrywania wiadomości
    hide_parser = subparsers.add_parser("hide", help="Hide secret message in text")
    hide_parser.add_argument("--template", action="store_true", help="Indicates that template is a file")
    hide_parser.add_argument("text", help="Text or template in which to hide the secret message")
    hide_parser.add_argument("message", help="Secret message to hide in the text")
    hide_parser.add_argument("output_file", help="Path to file with hidden text")

    # 3. Subparser do wydobywania wiadomości
    extract_parser = subparsers.add_parser("extract", help="Extract secret message from text")
    extract_parser.add_argument("--file", action="store_true", help="Indicates that hidden text is in a file")
    extract_parser.add_argument("text", help="Text containing the hidden secret message")
    args = parser.parse_args()

    if args.command == "hide":
        # 4. Ukrywanie wiadomości w tekście
        hidden_text = hide_message_in_text(args.text, args.message, args.output_file, template=args.template)
        print(hidden_text)
    elif args.command == "extract":
        # 5. Wydobywanie zakodowanej wiadomości
        extracted_message = extract_message_from_text(args.text, is_file=args.file)
        print(extracted_message)
    else:
        parser.print_help()
