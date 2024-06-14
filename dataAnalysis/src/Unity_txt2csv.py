import os
import pandas as pd

# Directory containing the txt files
directory = r'C:\Users\User\Desktop\6월11일_마지막\6월14일_Unity_Jumping'

# Lists to store data
gravity_acceleration = []
angular_velocity = []
linear_acceleration = []
Time = []

# Iterate over all files in the directory
for filename in os.listdir(directory):
    if filename.endswith('.txt'):
        filepath = os.path.join(directory, filename)
        with open(filepath, 'r') as file:
            for line in file:
                if "Time" in line:
                    Time.append(eval(line.split(': ')[1]))
                elif "Gravity Acceleration" in line:
                    gravity_acceleration.append(eval(line.split(': ')[1]))
                elif "Angular Velocity" in line:
                    angular_velocity.append(eval(line.split(': ')[1]))
                elif "Linear Acceleration" in line:
                    linear_acceleration.append(eval(line.split(': ')[1]))

# Create a DataFrame
df = pd.DataFrame({
    'Time' : Time,
    'Gravity Acceleration': gravity_acceleration,
    'Angular Velocity': angular_velocity,
    'Linear Acceleration': linear_acceleration
})

# Save to CSV
csv_path = '6월14일_유니티_jumping.csv'
df.to_csv(csv_path, index=False)

print(f"CSV file saved to {csv_path}")
