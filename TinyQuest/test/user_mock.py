class UserMock:
    
    def __init__(self, user_id, nickname):
        self._user_id = user_id
        self._nickname = nickname
    
    def user_id(self):
        return self._user_id

    def nickname(self):
        return self._nickname