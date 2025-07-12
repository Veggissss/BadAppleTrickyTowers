import os
import cv2

if __name__ == "__main__":
    FRAMES_DIR = "./frames"
    VIDEO_FILE = "./bad_apple.mp4"

    os.makedirs(FRAMES_DIR, exist_ok=True)

    video = cv2.VideoCapture(VIDEO_FILE)
    if not video.isOpened():
        print("Error: Could not open video file.")
        exit(1)

    frame_number = 0
    while video.isOpened():
        frame_number += 1
        ret, frame = video.read()
        print(f"Reading frame {frame_number}")
        if not ret:
            print("No more frames to read.")
            break
        cv2.imwrite(f"{FRAMES_DIR}/{frame_number}.jpg", frame)
    video.release()
