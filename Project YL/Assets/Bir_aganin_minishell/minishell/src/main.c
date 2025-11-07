/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   main.c                                             :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/04/16 20:45:28 by caide-so          #+#    #+#             */
/*   Updated: 2025/06/06 20:46:47 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../include/minishell.h"

void	prompt(t_shell *shell);
void	minishell(char *input, t_shell *shell);
void	parser(t_token_list *tokens, t_shell *shell);
int		handle_all_white_spaces(char *input);

// Global signal status variable.
// Tracks the current signal status for proper shell behavior.
volatile int		g_signal_status;

// Main minishell entry point.
// 1. Validates no argumentes are passed.
// 2. Initializes shell environment.
// 3. Starts interactive prompt.
// 4. Cleans up before exit.
int	main(int argc, char **argv, char **envp)
{
	t_shell	shell;

	if (argc > 1 || argv[1])
	{
		ft_putstr_fd("minishell: ", STDERR_FILENO);
		ft_putstr_fd(argv[1], STDERR_FILENO);
		ft_putstr_fd(": No such file or directory\n", STDERR_FILENO);
		return (127);
	}
	shell.env = init_env(envp);
	if (!shell.env)
	{
		ft_putstr_fd("minishell: failed to init environ\n", STDERR_FILENO);
		return (1);
	}
	shell.tokens = NULL;
	shell.ast = NULL;
	shell.last_status = 0;
	prompt(&shell);
	clean_all(NULL, NULL, &shell.env);
	return (0);
}

void	prompt(t_shell *shell)
{
	char	*input;
	char	*pmt;

	while (1)
	{
		pmt = make_prompt(shell);
		g_signal_status = -1;
		input = readline(pmt);
		free(pmt);
		if (g_signal_status == 130)
			update_sh_last_status(shell, -1);
		if (!input)
			break ;
		if (handle_all_white_spaces(input))
			continue ;
		if (ft_strcmp(input, "exit") == 0)
		{
			ft_putstr_fd("exit\n", STDERR_FILENO);
			free(input);
			break ;
		}
		add_history(input);
		minishell(input, shell);
		free(input);
	}
}

/*
// Alternative prompt supporting both interactive and non-interactive modes.
// 1. Handles input differently based on terminal status.
// 2. Processes input similarly to main prompt.
// 3. Maintains history in interactive mode.
// Uncomment this and comment other prompt function to use 42_minishell_tester.
void	prompt(t_shell *shell)
{
	char	*input;
	char	*pmt;
	char	*line;

	while (1)
	{
		if (isatty(fileno(stdin)))
		{
			// interactive: show prompt on stderr and read with readline()
			pmt = make_prompt(shell);
			g_signal_status = -1;
			input = readline(pmt);
			free(pmt);
			if (g_signal_status == 130)
				update_sh_last_status(shell, -1);
		}
		else
		{
			// non-interactive: read from stdin without printing any prompt
			line = get_next_line(fileno(stdin));
			if (!line)
				break ;
			input = ft_strtrim(line, "\n");
			free(line);
		}
		if (!input)
			break ;
		if (handle_all_white_spaces(input))
		{
			free(input);
			continue ;
		}
		if (ft_strcmp(input, "exit") == 0)
		{
			// in non-interactive, you still print "exit\n" to stdout
			// because bash does it; the tester expects it on stdout
			ft_putstr_fd("exit\n", STDOUT_FILENO);
			free(input);
			break ;
		}
		if (isatty(fileno(stdin)))
			add_history(input);
		minishell(input, shell);
		free(input);
	}
}
*/

void	minishell(char *input, t_shell *shell)
{
	t_token_list	*tokens;

	tokens = tokenizer(input);
	if (!tokens)
	{
		shell->last_status = 2;
		return ;
	}
	shell->tokens = tokens;
	parser(tokens, shell);
}

void	parser(t_token_list *tokens, t_shell *shell)
{
	t_ast	*ast;

	if (!syntax_analysis(tokens))
	{
		token_list_free(tokens);
		shell->last_status = 2;
		return ;
	}
	ast = parse_logical(&tokens->head, shell);
	shell->ast = ast;
	if (g_signal_status != -3)
		executor(tokens, shell, ast);
	token_list_free(tokens);
	ast_free(shell->ast);
	shell->ast = NULL;
	shell->tokens = NULL;
}

int	handle_all_white_spaces(char *input)
{
	if (input[0] == '\0' || is_all_whitespace(input))
	{
		free(input);
		return (1);
	}
	return (0);
}
