import unittest
import logging
from google.appengine.ext import db
import db_model
from ope_commands import *
from user_mock import UserMock

class OpeCommandTest(unittest.TestCase):

    def setUp(self):
        # Populate test entities.
        OpeCommands.create_master_data()

    def tearDown(self):
        # There is no need to delete test entities.
        pass

    def test_create_master_data(self):
        pass
