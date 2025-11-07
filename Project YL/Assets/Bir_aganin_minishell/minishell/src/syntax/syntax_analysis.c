/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   syntax_analysis.c                                  :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/04/30 19:22:39 by caide-so          #+#    #+#             */
/*   Updated: 2025/05/02 15:06:11 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

int	syntax_analysis(t_token_list *tokens)
{
	t_token	*curr;
	t_token	*prev;
	int		depth;

	depth = 0;
	if (tokens->size == 0)
		return (report_unexpected("newline"));
	curr = tokens->head;
	prev = NULL;
	if (curr->type == PIPE || curr->type == LOGICAL_AND
		|| curr->type == LOGICAL_OR)
		return (report_unexpected(curr->value));
	while (curr)
	{
		if (!tk_analysis(prev, curr, curr->next, &depth))
			return (0);
		prev = curr;
		curr = curr->next;
	}
	if (prev->type != WORD && prev->type != PAREN_CLOSE)
		return (report_unexpected("newline"));
	if (depth != 0)
		return (report_unexpected("newline"));
	return (1);
}

int	tk_analysis(t_token *prev, t_token *tk, t_token *nxt, int *dpt)
{
	if (tk->type == REDIR_IN || tk->type == REDIR_OUT
		|| tk->type == REDIR_APPEND || tk->type == HEREDOC)
		return (check_redir(nxt));
	else if (tk->type == PIPE)
		return (check_pipe(prev, nxt));
	else if (tk->type == LOGICAL_AND || tk->type == LOGICAL_OR)
		return (check_logical(prev, tk, nxt));
	else if (tk->type == PAREN_OPEN || tk->type == PAREN_CLOSE)
		return (check_paren(prev, tk, nxt, dpt));
	return (1);
}

int	is_all_whitespace(const char *s)
{
	int	i;

	i = 0;
	while (s[i])
	{
		if (s[i] != ' ' && s[i] != '\t')
			return (0);
		i++;
	}
	return (1);
}
