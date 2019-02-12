import faker from "faker";

describe('Login Test with Server', () => {

  const credentials = {
    name: "Barney Ullrich", 
    email: "Rodrick_Russel81@yahoo.com", 
    password: "OAsudlZIUTckj"
  };

  beforeEach(() => {
    cy.server();
    cy.visit('/login');
    cy.register(credentials);
  });
  
  afterEach(() => {
    cy.removeUser(credentials.email);
  });

  it('logs in succesfully and redirects to home', () => {
    cy.route('POST', `${Cypress.env('EXTERNAL_API')}/api/login`).as('login');
    cy.get('[data-cy=email]').clear().type(credentials.email);
    cy.get('[data-cy=password]').clear().type(credentials.password);
    cy.get('[data-cy=submit]').click();
    cy.wait('@login');
    cy.url().should('include', 'home');
  });

  it('doesnt log in because of wrong email and shows errors', () => {
    cy.route('POST', `${Cypress.env('EXTERNAL_API')}/api/login`).as('login');
    cy.get('[data-cy=email]').clear().type(faker.internet.email());
    cy.get('[data-cy=password]').clear().type(credentials.password);
    cy.get('[data-cy=submit]').click();
    cy.wait('@login');
    cy.get('[data-cy=errors]').should('be.visible');
    cy.url().should('include', 'login');
  });

  it('doesnt log in because of wrong password and shows errors', () => {
    cy.route('POST', `${Cypress.env('EXTERNAL_API')}/api/login`).as('login');
    cy.get('[data-cy=email]').clear().type(credentials.email);
    cy.get('[data-cy=password]').clear().type(faker.internet.password());
    cy.get('[data-cy=submit]').click();
    cy.wait('@login');
    cy.get('[data-cy=errors]').should('be.visible');
    cy.url().should('include', 'login');
  });});

describe('Login Form UI tests', () => {

  it('doesnt submit if information is missing', () => {
    cy.get('[data-cy=email]').clear().type(faker.internet.email());
    cy.get('[data-cy=submit]').click();
    cy.url().should('include','login');
  });
});
