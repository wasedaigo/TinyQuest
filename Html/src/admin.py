import os
from google.appengine.ext.webapp import template
from google.appengine.ext import webapp
from google.appengine.api import users
from ope_commands import OpeCommands

class AdminServer(webapp.RequestHandler):
  """Handles requests to /admin URLs and delegates to the Admin class."""
  def _initializeDB(self):
    OpeCommands.create_master_data()

  def get(self):
    """Ensure that the user is an admin."""
    if not users.GetCurrentUser():
      loginUrl = users.CreateLoginURL(self.request.uri)
      self.response.out.write('<a href="%s">Login</a>' % loginUrl)
      return
    
    if not users.IsCurrentUserAdmin():
      logoutUrl = users.CreateLogoutURL("/")
      text = '<a href="%s">Logout</a>' % logoutUrl
      self.response.out.write(text + 'You must be an admin to view this page.')
      return
    
    self._handleRequest()

  def _handleRequest(self):
    logoutUrl = users.create_logout_url(self.request.uri)

    action = self.request.get('action')
    msg = ''
    if action == 'initialize_db':
      self._initializeDB()
      msg = "Database has been initialized"

    logoutUrl = users.CreateLogoutURL("/")
    text = '<a href="%s">Logout</a>' % logoutUrl
    self.response.out.write('Admin Panel' + text)
