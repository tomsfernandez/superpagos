import faker from "faker";

describe('Login Form UI tests', () => {
  
  beforeEach(() => {
    cy.server();
    cy.visit('/');
  })

  it('doesnt submit if information is missing', () => {
    cy.get('[data-cy=email]').clear().type(faker.internet.email());
    cy.get('[data-cy=submit]').click();
    cy.url().should('include', 'login');
  });
});