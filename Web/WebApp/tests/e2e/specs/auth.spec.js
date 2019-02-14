import faker from "faker";

describe('Auth Test with Server', () => {

  const credentials = {
    name: "Barney Ullrich",
    email: "Rodrick_Russel81@yahoo.com",
    password: "OAsudlZIUTckj"
  };
  
  before(() => {
    cy.register(credentials);
  });

  beforeEach(() => {
    cy.server();
    cy.visit('/');
  });

  it('redirects to login if user attempts to visit protected site without authenticating', () => {
    cy.visit('home');
    cy.url().should('include', 'login');
  });

  it('logs in successfuly and redirects to method view', () => {
    cy.route('POST', `${Cypress.env('EXTERNAL_API')}/api/login`).as('login');
    cy.visit('home/methods');
    cy.get('[data-cy=email]').clear().type(credentials.email);
    cy.get('[data-cy=password]').clear().type(credentials.password);
    cy.get('[data-cy=submit]').click();
    cy.wait('@login');
    cy.url().should('include', 'home/methods');
  });
  
  it('is redirected to login because Barney is not admin', () => {
    cy.login(credentials);
    cy.visit('/home');
    cy.url().should('contain', 'home');
    cy.visit('/home/providers');
    cy.url().should('contain', 'login');
  });
  
  after(() => {
    cy.removeUser(credentials.email);
  })
});
