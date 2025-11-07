/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   tokenizer.c                                        :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/04/22 22:42:04 by caide-so          #+#    #+#             */
/*   Updated: 2025/05/02 15:10:30 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

t_token_list	*tokenizer(char *input)
{
	int				i;
	t_token_list	*tokens;
	int				result;

	i = 0;
	tokens = (t_token_list *)malloc(sizeof(t_token_list));
	if (!tokens)
		return (NULL);
	token_list_init(tokens);
	while (input[i])
	{
		result = process_input_char(input, &i, tokens);
		if (result == 0)
			return (NULL);
		if (result == 2)
			continue ;
		if (result == 1)
			continue ;
		if (!handle_word(input, &i, tokens))
		{
			token_list_free(tokens);
			return (NULL);
		}
	}
	return (tokens);
}

int	process_input_char(char *input, int *i, t_token_list *tokens)
{
	int	result;

	if (ft_isspace(input[*i]))
	{
		(*i)++;
		return (1);
	}
	result = process_operator(input, i, tokens);
	if (result == 0 || result == 2)
		return (result);
	if (!handle_word(input, i, tokens))
	{
		token_list_free(tokens);
		return (0);
	}
	return (1);
}

int	process_operator(char *input, int *i, t_token_list *tokens)
{
	int	op_result;

	op_result = handle_operators(input, i, tokens);
	if (op_result == 0)
	{
		token_list_free(tokens);
		return (0);
	}
	if (op_result == 1)
		return (2);
	return (1);
}

int	handle_word(char *input, int *i, t_token_list *tokens)
{
	int		j;
	int		start;
	char	*value;
	t_token	*token;

	start = *i;
	j = start;
	while (input[j] && !in("()&|<>", input[j]) && !ft_isspace(input[j]))
		if (!handle_quotes(input, &j))
			return (0);
	if (j == start)
		return (0);
	value = ft_substr(input, start, j - start);
	if (!value)
		exit_perror("ft_substr");
	token = new_token(WORD, value);
	if (!token)
	{
		free(value);
		exit_perror("new_token");
	}
	token_list_append(tokens, token);
	*i = j;
	return (1);
}

int	handle_quotes(char *input, int *j)
{
	char	type;

	if (input[*j] == '\'' || input[*j] == '\"')
	{
		type = input[*j];
		(*j)++;
		while (input[*j] && input[*j] != type)
			(*j)++;
		if (input[*j] != type)
		{
			report_unexpected_quotes((const char)type);
			return (0);
		}
		else
			(*j)++;
	}
	else
		(*j)++;
	return (1);
}
