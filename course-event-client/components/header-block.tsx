import styled from "styled-components";

export const HeaderBody = styled.div<{ $background?: string }>`
  width: 100%;
  background-color: ${({ $background }) => $background ?? "var(--background)"};
  box-shadow: 0 1px 5px 0px var(--boxShadow);
  color: var(--text);
  border-radius: 16px;
  padding: 20px 24px;
  display: flex;
  gap: 16px;
  justify-content: space-between;
  /* align-items: center; */
  & > * {
    color: var(--color-text);
  }

  @media (max-width: 847px) {
    flex-direction: column;
  }

  ::-webkit-scrollbar {
    height: 10px;
  }

  ::-webkit-scrollbar-track {
    background-color: rgba(231, 231, 232, 0.736);
    border-radius: 100px;
  }

  ::-webkit-scrollbar-thumb {
    background-color: var(--boxShadow);
    border: 1px solid var(--background);
    border-radius: 100px;
  }
`;