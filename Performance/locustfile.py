from locust import HttpLocust, TaskSet, task

import settings


class LocustTaskSet(TaskSet):

    @task
    def post_points(self):
        self.client.get("http://localhost:5000/api/health")


class LocustUser(HttpLocust):
    task_set = LocustTaskSet
    min_wait = settings.MIN_WAIT
    max_wait = settings.MAX_WAIT
