// type definitions for Cypress object "cy"
/// <reference types="cypress" />

import faker from "faker";

describe('Register Test with Server', () => {

  const credentials = {
    name: "Barney Ullrich", 
    email: "Rodrick_Russel81@yahoo.com", 
    password: "OAsudlZIUTckj"
  };

  beforeEach(() => {
    cy.server();
    cy.visit('/register');
  });
  
  afterEach(() => {
    cy.removeUser(credentials.email);
  });

  it('registers user that doesnt exist', () => {
    cy.route('POST', '/api/users').as('register');
    cy.get('[data-cy=name]').clear().type(credentials.name);
    cy.get('[data-cy=email]').clear().type(credentials.email);
    cy.get('[data-cy=password]').clear().type(credentials.password);
    cy.get('[data-cy=submit]').click();
    cy.wait('@register');
    cy.url().should('include', 'login');
  });

  it('shows error if user is registered with taken email', () => {
    cy.register(credentials);
    cy.route('POST', '/api/users').as('register');
    cy.get('[data-cy=name]').clear().type(faker.name.findName());
    cy.get('[data-cy=email]').clear().type(credentials.email);
    cy.get('[data-cy=password]').clear().type(faker.internet.password());
    cy.get('[data-cy=submit]').click();
    cy.wait('@register');
    cy.url().should('include', 'register');
    cy.get('[data-cy=error-div]').should('be.visible');
  });
});

describe('Register Form UI tests', () => {

  beforeEach(() => {
    cy.visit('/register');
  });

  it('doesnt submit if information is missing', () => {
    cy.get('[data-cy=email]').clear().type(faker.internet.email());
    cy.get('[data-cy=password]').clear().type(faker.internet.password());
    cy.get('[data-cy=submit]').click();
    cy.url().should('include','register');
  });

  it('doesnt submit if email is not valid', () => {
    cy.get('[data-cy=name]').clear().type(faker.name.findName());
    cy.get('[data-cy=email]').clear().type("anInvalidEmail");
    cy.get('[data-cy=password]').clear().type(faker.internet.password());
    cy.get('[data-cy=submit]').click();
    cy.url().should('include','register');
  });
});