// type definitions for Cypress object "cy"
/// <reference types="cypress" />

import faker from "faker";

describe('Register Test with Server', () => {

  const credentials = {
    name: "Barney Ullrich", 
    email: "Rodrick_Russel81@yahoo.com", 
    password: "OAsudlZIUTckj"
  };

  before(() => {
    cy.server();
  });

  beforeEach(() => {
    cy.visit('/register');
    cy.removeUser(credentials.email);
  }); 

  it('registers user that doesnt exist', () => {
    cy.route('POST', '/api/users').as('register');
    cy.get('[data-cy=name]').type(credentials.name);
    cy.get('[data-cy=email]').type(credentials.email);
    cy.get('[data-cy=password]').type(credentials.password);
    cy.get('[data-cy=submit]').click();
    cy.wait('@register');
    cy.url().should('include', 'login');
  })

  it('shows error if user is registered with taken email', () => {
    cy.register(credentials);
    cy.route('POST', '/api/users').as('register');
    cy.get('[data-cy=name]').type(faker.name.findName());
    cy.get('[data-cy=email]').type(credentials.email);
    cy.get('[data-cy=password]').type(faker.internet.password());
    cy.get('[data-cy=submit]').click();
    cy.wait('@register');
    cy.url().should('include', 'register');
  });
});

describe('Register Form UI tests', () => {

  it('doesnt submit if information is missing', () => {
    cy.get('[data-cy=name]').type(credentials.name);
    cy.get('[data-cy=email]').type(credentials.email);
    cy.get('[data-cy=password]').type(credentials.password);
    cy.get('[data-cy=submit]').click();
  });

  it('doesnt submit if email is not valid', () => {

  });
});
