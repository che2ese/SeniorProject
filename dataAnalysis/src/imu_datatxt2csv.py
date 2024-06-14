import csv

input_file_path = r'C:\Users\User\Desktop\6월11일_마지막\6월14일_IMU_Jumping.csv'  # 입력 파일을 저장한 경로
output_file_path = '6월14일_IMU_jumping_processed.csv'  # 추출한 데이터를 저장할 경로

# 입력 파일을 읽고 데이터를 처리
with open(input_file_path, 'r') as infile, open(output_file_path, 'w', newline='') as outfile:
    fieldnames = ['angular_velocity_x', 'angular_velocity_y', 'angular_velocity_z', 
                  'linear_acceleration_x', 'linear_acceleration_y', 'linear_acceleration_z']
    writer = csv.DictWriter(outfile, fieldnames=fieldnames)
    writer.writeheader()

    # 전체 파일을 읽어서 --- 로 구분된 각 블록을 처리
    data = infile.read()
    segments = data.strip().split('---')

    for segment in segments:
        lines = segment.strip().split('\n')
        ang_vel = {}
        lin_acc = {}

        for i, line in enumerate(lines):
            if 'angular_velocity:' in line:
                ang_vel['x'] = float(lines[i+1].split(': ')[1])
                ang_vel['y'] = float(lines[i+2].split(': ')[1])
                ang_vel['z'] = float(lines[i+3].split(': ')[1])
            if 'linear_acceleration:' in line:
                lin_acc['x'] = float(lines[i+1].split(': ')[1])
                lin_acc['y'] = float(lines[i+2].split(': ')[1])
                lin_acc['z'] = float(lines[i+3].split(': ')[1])
        
        if ang_vel and lin_acc:
            writer.writerow({
                'angular_velocity_x': ang_vel['x'],
                'angular_velocity_y': ang_vel['y'],
                'angular_velocity_z': ang_vel['z'],
                'linear_acceleration_x': lin_acc['x'],
                'linear_acceleration_y': lin_acc['y'],
                'linear_acceleration_z': lin_acc['z']
            })

print(f"CSV 파일 생성 완료: {output_file_path}")