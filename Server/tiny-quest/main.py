import sys

sys.path.append('./src')

from google.appengine.ext import webapp
from google.appengine.ext.webapp.util import run_wsgi_app
from api import ApiServer

application = webapp.WSGIApplication(
                                     [
                                     ('/api/FindQuickMatch',  ApiServer),
                                     ('/api/start_battle',  ApiServer),
                                     ('/api/progress_turn',  ApiServer)
                                     ],
                                     debug=True)

def main():
    run_wsgi_app(application)

if __name__ == "__main__":
    main()
