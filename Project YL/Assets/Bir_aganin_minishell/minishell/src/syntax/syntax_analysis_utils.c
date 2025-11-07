/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   syntax_analysis_utils.c                            :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/02 14:26:12 by caide-so          #+#    #+#             */
/*   Updated: 2025/05/02 14:32:23 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

int	check_pipe(t_token *prev, t_token *next)
{
	if (!prev || (prev->type != WORD && prev->type != PAREN_CLOSE))
		return (report_unexpected("|"));
	if (!next || (next->type != WORD && next->type != PAREN_OPEN
			&& next->type != REDIR_IN && next->type != REDIR_OUT
			&& next->type != REDIR_APPEND && next->type != HEREDOC))
		return (report_unexpected("|"));
	return (1);
}

int	check_redir(t_token *next)
{
	const char	*off;

	if (next)
		off = next->value;
	else
		off = "newline";
	if (!next || next->type != WORD)
		return (report_unexpected(off));
	return (1);
}

int	check_logical(t_token *prev, t_token *token, t_token *next)
{
	const char	*sym;

	if (is_twochar(token) && token->type == LOGICAL_OR)
		sym = "||";
	else
		sym = "&&";
	if (!prev || (prev->type != WORD && prev->type != PAREN_CLOSE))
		return (report_unexpected(sym));
	if (next && (next->type == PIPE || next->type == LOGICAL_AND
			|| next->type == LOGICAL_OR || next->type == PAREN_CLOSE))
		return (report_unexpected(next->value));
	return (1);
}

int	is_twochar(t_token *token)
{
	return (token->type == LOGICAL_AND || token->type == LOGICAL_OR
		|| token->type == REDIR_APPEND || token->type == HEREDOC);
}

int	check_paren(t_token *prev, t_token *token, t_token *next, int *depth)
{
	if (token->type == PAREN_OPEN)
	{
		if (next && next->type == PAREN_CLOSE)
			return (report_unexpected(")"));
		if (!next || (next->type != WORD && next->type != PAREN_OPEN))
			return (report_unexpected("("));
		(*depth)++;
	}
	else
	{
		if (!prev || (prev->type != WORD && prev->type != PAREN_CLOSE))
			return (report_unexpected(")"));
		(*depth)--;
		if (*depth < 0)
			return (report_unexpected(")"));
	}
	return (1);
}
