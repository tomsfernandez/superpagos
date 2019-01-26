FROM python:3.7-alpine

RUN apk add build-base

WORKDIR /app
COPY ./locust .

RUN pip install locustio==0.8.1

ENTRYPOINT ["python", "run.py"]