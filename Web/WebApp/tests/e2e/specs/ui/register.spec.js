import faker from "faker";

describe('Register Form UI tests', () => {

  beforeEach(() => {
    cy.visit('/register');
  });

  it('doesnt submit if information is missing', () => {
    cy.get('[data-cy=email]').clear().type(faker.internet.email());
    cy.get('[data-cy=password]').clear().type(faker.internet.password());
    cy.get('[data-cy=submit]').click();
    cy.url().should('include', 'register');
  });

  it('doesnt submit if email is not valid', () => {
    cy.get('[data-cy=name]').clear().type(faker.name.findName());
    cy.get('[data-cy=email]').clear().type("anInvalidEmail");
    cy.get('[data-cy=password]').clear().type(faker.internet.password());
    cy.get('[data-cy=submit]').click();
    cy.url().should('include', 'register');
  });
});