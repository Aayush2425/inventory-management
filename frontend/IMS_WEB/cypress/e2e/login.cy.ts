/// <reference types="cypress" />

describe('Login Page - E2E', () => {
  it('should login successfully and redirect to dashboard', () => {
    cy.visit('http://localhost:4200/Login');

    cy.get('input[formcontrolname="username"]').type('OWNER');
    cy.get('input[formcontrolname="password"]').type('OWNER123');
    cy.get('button[type="submit"]').click();

    cy.url().should('include', '/dashboard');
    cy.contains('Welcome').should('exist');
  });
});
