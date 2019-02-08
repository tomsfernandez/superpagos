// type definitions for Cypress object "cy"
/// <reference types="cypress" />

describe('Example Test', () => {
  it('Visits the app root url', () => {
    cy.visit('/');
    cy.withStore(store => expect(store).to.not.equal(undefined));
  })
});
