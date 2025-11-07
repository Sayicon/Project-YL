/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   expand_wild_utils.c                                :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: marcudos <marcudos@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/27 16:29:40 by marcudos          #+#    #+#             */
/*   Updated: 2025/06/01 18:38:47 by marcudos         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

int	match_middle(char *pos, t_wild *wild, int i, int limit)
{
	size_t	part_len;
	size_t	end_len;

	if (wild->have_end)
		end_len = ft_strlen(wild->parts[limit]);
	else
		end_len = 0;
	while (i < limit)
	{
		if (ft_strlen(pos) <= end_len)
			return (0);
		part_len = ft_strlen(wild->parts[i]);
		pos = ft_strnstr(pos, wild->parts[i], ft_strlen(pos) - end_len);
		if (!pos)
			return (0);
		pos += part_len;
		i++;
	}
	return (1);
}

int	match_end(const char *filename, const char *end)
{
	size_t	file_len;
	size_t	end_len;

	file_len = ft_strlen(filename);
	end_len = ft_strlen(end);
	if (file_len < end_len)
		return (0);
	return (ft_strncmp(filename + file_len - end_len, end, end_len) == 0);
}

int	match_pattern(const char *filename, t_wild *wild)
{
	char	*pos;
	int		i;
	int		len;

	i = 0;
	if (wild->full)
		return (1);
	pos = (char *) filename;
	if (!filename || !wild->parts)
		return (0);
	len = count_array((void **) wild->parts);
	if (wild->have_start)
	{
		if (ft_strncmp(filename, wild->parts[0],
				ft_strlen(wild->parts[0])) != 0)
			return (0);
		pos = (char *) filename + ft_strlen(wild->parts[0]);
		i++;
	}
	if (!match_middle(pos, wild, i, len - wild->have_end))
		return (0);
	if (wild->have_end)
		return (match_end(filename, wild->parts[len - 1]));
	return (1);
}

int	handle_opening_quote(char *input, int *i, t_quote *state)
{
	if ((input[*i] == '\'' && *state == NO_QUOTE))
	{
		*state = SINGLE_QUOTE;
		(*i)++;
		return (1);
	}
	else if ((input[*i] == '\"' && *state == NO_QUOTE))
	{
		*state = DOUBLE_QUOTE;
		(*i)++;
		return (1);
	}
	return (0);
}

int	update_state_quote_i(char *input, int *i, t_quote *state)
{
	int	handled;

	handled = 0;
	while (input[*i] == '\'' || input[*i] == '\"')
	{
		if ((input[*i] == '\'' && *state == SINGLE_QUOTE)
			|| (input[*i] == '\"' && *state == DOUBLE_QUOTE))
		{
			(*i)++;
			*state = NO_QUOTE;
			handled = 1;
		}
		else if (input[*i] == '\'' || input[*i] == '\"')
		{
			if (handle_opening_quote(input, i, state))
				handled = 1;
			else
				break ;
		}
		else
			break ;
	}
	return (handled);
}
