import os
import re

data_directory = r'C:\Users\admin\Documents\카카오톡 받은 파일\SeniorProject\My project\Assets\DataLogs'
file_prefix = 'ldata_'
output_prefix = 'walking'
#output_prefix = 'running'
#output_prefix = 'crouch'
#output_prefix = 'jump'
file_extension = '.txt'


def parse_log_data(log_data):
    entries = log_data.strip().split("\n\n")
    parsed_entries = []
    for entry in entries:
        lines = entry.split("\n")
        time_line = lines[0]
        time_value = float(time_line.split(": ")[1])
        parsed_entries.append((time_value, entry))
    return parsed_entries


def extract_file_index(filename):
    match = re.search(r'(\d+)', filename)
    return int(match.group(1)) if match else -1


# Get the list of bata files in the directory and sort them by index
files = sorted([f for f in os.listdir(data_directory) if f.startswith(file_prefix) and f.endswith(file_extension)],
               key=extract_file_index)

# Combine and sort the files
for i in range(len(files) - 1):
    first_file_path = os.path.join(data_directory, files[i])
    second_file_path = os.path.join(data_directory, files[i + 1])
    combined_file_path = os.path.join(data_directory, f'{output_prefix}{i + 1}{file_extension}')

    all_entries = []

    # Read the first file
    with open(first_file_path, 'r') as first_file:
        first_data = first_file.read()
        all_entries.extend(parse_log_data(first_data))

    # Read the second file
    with open(second_file_path, 'r') as second_file:
        second_data = second_file.read()
        all_entries.extend(parse_log_data(second_data))

    # Sort entries by time
    all_entries.sort(key=lambda x: x[0])

    # Write sorted entries to the combined file
    with open(combined_file_path, 'w') as combined_file:
        for _, entry in all_entries:
            combined_file.write(entry + "\n\n")

print('Files combined and sorted successfully.')


