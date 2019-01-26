import os
from dotenv import load_dotenv

if not os.getenv("DOCKER_ENV"):
    load_dotenv(verbose=True)

MIN_WAIT = os.getenv("MIN_WAIT") if os.getenv("MIN_WAIT") else 200
MAX_WAIT = os.getenv("MAX_WAIT") if os.getenv("MAX_WAIT") else 200
API_HOST = os.getenv("API_HOST") if os.getenv("API_HOST") else "http://localhost:5000"
LOCUST_USERS = os.getenv("LOCUST_USERS") if os.getenv("LOCUST_USERS") else "LocustUser"

print(f"MIN_WAIT={MIN_WAIT}")
print(f"MAX_WAIT={MAX_WAIT}")
print(f"API_HOST={API_HOST}")
print(f"LOCUST_USERS={LOCUST_USERS}")
