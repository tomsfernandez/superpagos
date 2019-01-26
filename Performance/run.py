import os
import subprocess
import sys
import typing
from argparse import ArgumentParser

import settings


def parse(args: list = None) -> dict:
    configs: dict = {}

    parser = ArgumentParser()

    parser.add_argument(
        '--master-host',
        dest='master_host',
        default=None,
        help='The hostname/ip of the master instance'
    )

    configs.update(vars(parser.parse_args(args)))
    configs['is_master'] = (configs['master_host'] is None)
    return configs


def get_locust_file() -> typing.Union[str, None]:
    return "locustfile.py"


def make_command(args: dict) -> str:
    api_host = settings.API_HOST
    # cmd = ['locust', '-f', f"{get_locust_file()}",
    #        '--master' if args['is_master'] else '--slave',
    #        f"--host={api_host}"]
    cmd = ['locust', '-f', f"{get_locust_file()}", f"--host={api_host}"]

    if args.get('is_master'):
        cmd.append(f"--slave --master-host={args['master_host']}")
    if args.get('is_slave'):
        cmd.append(f"--master")
    try:
        locust_users = settings.LOCUST_USERS
        cmd += locust_users.split(',')
    except Exception:
        print('[ERROR]: LOCUST_USERS variable must be separated by comma')
        raise

    toReturn = ' '.join(cmd)
    print(toReturn)
    return toReturn


def run():
    args = parse()

    process = subprocess.Popen(
        args=make_command(args),
        stdout=sys.stdout,
        shell=True,
        universal_newlines=True
    )

    process.wait()


if __name__ == '__main__':
    run()
